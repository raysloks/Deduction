using System.Net;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using EventCallbacks;

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
                    text.text = names[mob];
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
                UpdateName(message.id);
            }
            mobs[message.id].gameObject.SetActive(true);
            if (mobs[message.id] is NetworkMob mob)
                mob.AddSnapshot(new NetworkMob.Snapshot { time = message.time, position = message.position });
        }
    }

    internal void PlayerUpdateHandler(IPEndPoint endpoint, PlayerUpdate message)
    {
        if (message.id == ulong.MaxValue)
        {
            playerMobId = message.mob;
            mobs[playerMobId] = game.player;
            game.connectionState = GameController.ConnectionState.Connected;
        }
        else
            names[message.mob] = message.name;
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
        EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.MEETING_VOTED, uvei);
    }

    internal void GameStartRequestedHandler(IPEndPoint endpoint, GameStartRequested message)
    {
    }

    internal void RestartRequestedHandler(IPEndPoint endpoint, RestartRequested message)
    {
    }

    internal void MobRemovedHandler(IPEndPoint endpoint, MobRemoved message)
    {
        if (mobs.ContainsKey(message.id) && message.id != playerMobId)
            mobs[message.id].gameObject.SetActive(false);
        removalTimes[message.id] = message.time;
    }

    internal void KillAttemptedHandler(IPEndPoint endpoint, KillAttempted message)
    {
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
            mob.sprite.color = new Color(message.color.x, message.color.y, message.color.z);
        }
    }

    internal void VoiceFrameHandler(IPEndPoint endpoint, VoiceFrame message)
    {
        VoicePlayer voicePlayer = null;
        if (mobs.ContainsKey(message.id) && (message.id != playerMobId || game.listenToSelf))
            voicePlayer = mobs[message.id].GetComponentInChildren<VoicePlayer>();
        if (voicePlayer)
            voicePlayer.ProcessFrame(message.data);
    }

    internal void ConnectionHandler(IPEndPoint endpoint)
    {
        game.connectionState = GameController.ConnectionState.JoiningLobby;

        string name = game.nameInputField.text;
        name = name.Trim();
        if (name.Length == 0)
            name = "Agent " + game.rng.RangeInt(1, 1000).ToString().PadLeft(3, '0');
        link.Send(new PlayerUpdate { name = name });
    }

    internal void TaskListUpdateHandler(IPEndPoint endpoint, TaskListUpdate message)
    {
        game.taskManager.tasks.Clear();
        foreach (var task in message.tasks)
            game.taskManager.tasks.Add(new Task { minigame_index = task, completed = false });
    }

    internal void TaskUpdateHandler(IPEndPoint endpoint, TaskUpdate message)
    {
    }

    internal void GameOverHandler(IPEndPoint endpoint, GameOver message)
    {
        if (message.winners.Contains(playerMobId))
        {
            Debug.Log("VICTORY");
            SoundEvent se = new SoundEvent();
            se.EventDescription = "GAME WON";
            se.UnitSound = game.gameWinSounds;
            se.UnitGameObjectPos = game.player.transform.position;
            EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);
        }
        else
        {
            Debug.Log("VICTORY");
            SoundEvent se = new SoundEvent();
            se.EventDescription = "GAME LOST";
            se.UnitSound = game.gameLostSounds;
            se.UnitGameObjectPos = game.player.transform.position;
            EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);
            Debug.Log("DEFEAT");
        }
    }

    internal void GivenTasksHandler(IPEndPoint endpoint, GivenTasks message)
    {
    }

    internal void GameSettingsHandler(IPEndPoint endpoint, GameSettings message)
    {
        game.settings = message;
        game.settingsManager.UpdateInputDisplay();
    }

    internal void ResetGameSettingsHandler(IPEndPoint endpoint, ResetGameSettings message)
    {
    }

    internal void DoorUpdateHandler(IPEndPoint endpoint, DoorUpdate message)
    {
    }

    internal void LightUpdateHandler(IPEndPoint endpoint, LightUpdate message)
    {
        throw new NotImplementedException();
    }
}