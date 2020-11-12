using System.Net;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using EventCallbacks;
using System.Linq;
using System.IO;
using System.IO.Compression;

using System.Text;

public class NetworkHandler
{
    public GameController game;

    public NetLink link;

    public Dictionary<ulong, Mob> mobs = new Dictionary<ulong, Mob>();
    public Dictionary<ulong, string> names = new Dictionary<ulong, string>();
    public Dictionary<ulong, ulong> roles = new Dictionary<ulong, ulong>();
    public Dictionary<ulong, long> removalTimes = new Dictionary<ulong, long>();

    public ulong playerMobId = ulong.MaxValue;

    public int port = 16343;

    public NetworkHandler()
    {
        link = new NetLink();
        link.handler = this;
        link.Open(new IPEndPoint(IPAddress.Any, 0));
        link.Connect(new IPEndPoint(IPAddress.Parse("172.105.79.194"), port));
        link.Receive();
    }

    private void UpdateName(ulong mob)
    {
        TextMeshProUGUI text;
        if (mobs.ContainsKey(mob))
        {
            text = mobs[mob].GetComponentInChildren<TextMeshProUGUI>();
            if (text)
            {
                if (names.ContainsKey(mob))
                {
                    text.text = names[mob];
                    mobs[mob].name = names[mob];
                }
                if (roles.ContainsKey(mob))
                    text.color = roles[mob] == 1 ? Color.red : Color.white;
            }
            if (roles.ContainsKey(mob))
                mobs[mob].role = roles[mob];
        }
    }

    internal void MobUpdateHandler(IPEndPoint endpoint, MobUpdate message)
    {
        if (removalTimes.ContainsKey(message.id) && removalTimes[message.id] >= message.time)
            return;
        if (message.id != playerMobId)
        {
            if (!mobs.ContainsKey(message.id))
            {
                mobs.Add(message.id, UnityEngine.Object.Instantiate(game.prefab, message.position, Quaternion.identity).GetComponent<NetworkMob>());
                UnityEngine.Object.DontDestroyOnLoad(mobs[message.id].gameObject);
                UpdateName(message.id);
            }
            Mob mob = mobs[message.id];
            if (!mob.gameObject.activeSelf)
            {
                mob.gameObject.SetActive(true);
                mob.transform.position = message.position;
            }
            if (mob is NetworkMob networkMob)
                networkMob.AddSnapshot(new NetworkMob.Snapshot { time = message.time, position = message.position });
        }
    }

    internal void PlayerUpdateHandler(IPEndPoint endpoint, PlayerUpdate message)
    {
        if (message.id >= ulong.MaxValue - 1)
        {
            game.IsLeader = message.id == ulong.MaxValue;
            playerMobId = message.mob;
            mobs[playerMobId] = game.player;
            game.connectionState = GameController.ConnectionState.Connected;
            game.timeout = game.time + 5000000000;
        }
        else
        {
            names[message.mob] = message.name;
            if (message.name.Length == 0)
                names.Remove(message.mob);
        }
        UpdateName(message.mob);
    }

    internal void HeartbeatHandler(IPEndPoint endpoint, Heartbeat message)
    {
        if (Math.Abs(game.time - message.time) > 50000000)
            game.time = message.time;
        game.timeout = message.time + 5000000000;
    }

    internal void MobTeleportHandler(IPEndPoint endpoint, MobTeleport message)
    {
        if (mobs.ContainsKey(message.id))
        {
            Mob mob = mobs[message.id];
            mob.transform.position = message.to;
            if (mob is NetworkMob networkMob)
                networkMob.snapshots.Clear();
        }
    }

    internal void GamePhaseUpdateHandler(IPEndPoint endpoint, GamePhaseUpdate message)
    {      
        game.SetGamePhase((GamePhase)message.phase, message.timer, (GamePhase)message.previous);
    }

    internal void MeetingRequestedHandler(IPEndPoint endpoint, MeetingRequested message)
    {
        MeetingEvent umei = new MeetingEvent();
        umei.game = game;
        umei.idOfInitiator = message.idOfInitiator;

        if (message.idOfInitiator == playerMobId)
            game.player.emergencyButtonsLeft -= 1;

        umei.EventDescription = "Meeting Got Started";
        EventSystem.Current.FireEvent(EVENT_TYPE.MEETING_STARTED, umei);
    }

    internal void PlayerVotedHandler(IPEndPoint endpoint, PlayerVoted message)
    {
        VoteEvent uvei = new VoteEvent();
        uvei.idOfVoter = message.voter;
        uvei.idOfTarget = message.target;
        uvei.EventDescription = "Player Voted";
        EventSystem.Current.FireEvent(EVENT_TYPE.MEETING_VOTED, uvei);
    }

    internal void GameStartRequestedHandler(IPEndPoint endpoint, GameStartRequested message)
    {
    }

    internal void RestartRequestedHandler(IPEndPoint endpoint, RestartRequested message)
    {
        Debug.Log("Enter Evidence Handler");
    }

    internal void MobRemovedHandler(IPEndPoint endpoint, MobRemoved message)
    {
        if (mobs.ContainsKey(message.id) && message.id != playerMobId)
            mobs[message.id].gameObject.SetActive(false);
        removalTimes[message.id] = message.time;
    }

    internal void KillAttemptedHandler(IPEndPoint endpoint, KillAttempted message)
    {
        if (message.target == playerMobId)
            game.deathAnimation.Play(mobs[message.target], mobs[message.killer]);
        if (message.killer == playerMobId || message.killer == ulong.MaxValue)
            game.player.killCooldown = message.time;
    }

    internal void ReportAttemptedHandler(IPEndPoint endpoint, ReportAttempted message)
    {
        MeetingEvent umei = new MeetingEvent();
        umei.game = game;
        umei.idOfInitiator = message.idOfInitiator;
        umei.idOfBody = message.target;
        umei.EventDescription = "BodyReported";
        EventSystem.Current.FireEvent(EVENT_TYPE.MEETING_STARTED, umei);
    }

    internal void AbilityUsedHandler(IPEndPoint endpoint, AbilityUsed message)
    {
        game.player.sabotageCooldown = message.time;
    }

    internal void MobRoleUpdateHandler(IPEndPoint endpoint, MobRoleUpdate message)
    {
        roles[message.id] = message.role;
        UpdateName(message.id);
    }

    internal void MobStateUpdateHandler(IPEndPoint endpoint, MobStateUpdate message)
    {
        MobUpdateHandler(endpoint, message.update);
        if (mobs.ContainsKey(message.update.id))
        {
            Mob mob = mobs[message.update.id];
            mob.SetType(message.type);
            mob.SetSprite(message.sprite);
            //mob.sprite.color = new Color(message.color.x, message.color.y, message.color.z);
        }
    }

    internal void VoiceFrameHandler(IPEndPoint endpoint, VoiceFrame message)
    {
        VoicePlayer voicePlayer = null;
        if (mobs.ContainsKey(message.id))
            voicePlayer = mobs[message.id].GetComponentInChildren<VoicePlayer>();
        if (voicePlayer)
            voicePlayer.ProcessFrame(message.data);
    }

    internal void ConnectionHandler(IPEndPoint endpoint)
    {
        game.connectionState = GameController.ConnectionState.JoiningLobby;
        game.timeout = game.time + 2000000000;
        game.UpdateName();
    }

    internal void TaskListUpdateHandler(IPEndPoint endpoint, TaskListUpdate message)
    {
        game.taskManager.tasks.Clear();
        foreach (var task in message.tasks)
            game.taskManager.tasks.Add(new Task { minigame_index = task, completed = false });
        PasswordSpawner passwordSpawner = UnityEngine.Object.FindObjectOfType<PasswordSpawner>();
        if (passwordSpawner != null)
            passwordSpawner.SetPassword(message.password, message.passwordSuffix, message.passwordLocation);
    }

    internal void TaskUpdateHandler(IPEndPoint endpoint, TaskUpdate message)
    {
        game.taskbar.maxValue = (names.Count - game.settings.impostorCount) * (game.settings.shortTaskCount + game.settings.longTaskCount);
        game.taskbar.value = message.task;
    }

    internal void GameOverHandler(IPEndPoint endpoint, GameOver message)
    {
        GameOverEvent gameOverEvent = new GameOverEvent();
        gameOverEvent.winners = message.winners.Select(x => mobs[x]).ToList();
        gameOverEvent.victory = message.winners.Contains(playerMobId);
        gameOverEvent.role = message.role;
        EventSystem.Current.FireEvent(EVENT_TYPE.GAME_OVER, gameOverEvent);

        SoundEvent se = new SoundEvent();
        se.UnitGameObjectPos = game.player.transform.position;
        if (gameOverEvent.victory == true)
        {
            se.UnitSound = game.gameWinSounds;
        }
        else
        {        
            se.UnitSound = game.gameLostSounds;
        }
        EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);
    }

    internal void GivenTasksHandler(IPEndPoint endpoint, GivenTasks message)
    {
    }

    internal void GameSettingsHandler(IPEndPoint endpoint, GameSettings message)
    {
        game.settings = message;
        game.mapManager.CheckForMapChange();
        game.settingsManager.UpdateInputDisplay();
    }

    internal void ResetGameSettingsHandler(IPEndPoint endpoint, ResetGameSettings message)
    {
    }

    internal void DoorUpdateHandler(IPEndPoint endpoint, DoorUpdate message)
    {
        game.doorManager.SetDoorState(message.door, message.open);
    }

    internal void LightUpdateHandler(IPEndPoint endpoint, LightUpdate message)
    {
        game.lightTarget = message.light;
    }

    internal void MobEjectedHandler(IPEndPoint endpoint, MobEjected message)
    {
        EventSystem.Current.FireEvent(EVENT_TYPE.MOB_EJECTED, new MobEjectedEvent { mob = mobs[message.id] });
    }

    internal void SabotageTaskUpdateHandler(IPEndPoint endpoint, SabotageTaskUpdate message)
    {
        game.taskManager.sabotageTasks.RemoveAll(x => x.sabotage == message.sabotage);
        if (!message.completed)
        {
            SabotageTask task = new SabotageTask();
            task.sabotage = message.sabotage;
            task.minigame_index = message.minigame_index;
            task.timer = message.timer;
            game.taskManager.sabotageTasks.Add(task);
        }
    }
    internal void SendEvidenceHandler(IPEndPoint endpoint, SendEvidence message)
    {
        SendEvidenceEvent sendEvidenceEvent = new SendEvidenceEvent();
        if(message.picturePos != null)
        {
            Debug.Log("This is the player " + message.id + " count vec3 vs mobs "+ message.picturePos.Count+ " VS " + mobs.Count);
            sendEvidenceEvent.vec3List = message.picturePos;
            int index = 0;
            foreach(Mob m in mobs.Values)
            {
               // m.transform.position = message.picturePos[index];
                index++;
            }
            sendEvidenceEvent.Evidence = 1;
            sendEvidenceEvent.idOfTarget = message.id;
            sendEvidenceEvent.positionOfTarget = message.IntiatorPos;
            sendEvidenceEvent.gc = game;
        }
        else
        {
            Debug.Log("No Pos");
        }
        EventSystem.Current.FireEvent(EVENT_TYPE.SEND_EVIDENCE, sendEvidenceEvent);
    }

}