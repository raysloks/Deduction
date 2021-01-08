using System.Net;
using System.Collections;
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

    public void Reset()
    {
        foreach (var n in mobs)
            if (!(n.Value is Player))
                UnityEngine.Object.Destroy(n.Value.gameObject);
        mobs.Clear();
        names.Clear();
        roles.Clear();
        removalTimes.Clear();
        playerMobId = ulong.MaxValue;
    }

    private void UpdateName(ulong mob)
    {
        TextMeshPro text;
        if (mobs.ContainsKey(mob))
        {
            text = mobs[mob].GetComponentInChildren<TextMeshPro>();
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
                mobs.Add(message.id, UnityEngine.Object.Instantiate(game.prefab, message.position, Quaternion.identity, game.mobContainer).GetComponent<NetworkMob>());
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
        Debug.Log("Meeting Requested");

        MeetingEvent umei = new MeetingEvent();
        umei.game = game;
        umei.idOfInitiator = message.idOfInitiator;

        if (message.idOfInitiator == playerMobId)
            game.player.emergencyButtonsLeft -= 1;
        game.player.inLocker = false;
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
    }

    internal void MobRemovedHandler(IPEndPoint endpoint, MobRemoved message)
    {
        if (mobs.ContainsKey(message.id) && message.id != playerMobId)
        {
            mobs[message.id].type = 2;
            mobs[message.id].gameObject.SetActive(false);
        }
        removalTimes[message.id] = message.time;
    }

    internal void KillAttemptedHandler(IPEndPoint endpoint, KillAttempted message)
    {
        Debug.Log("Kill");
        if (message.target == playerMobId)
            game.deathAnimation.Play(mobs[message.target], mobs[message.killer]);
        if (message.knife == false && (message.killer == playerMobId || message.killer == ulong.MaxValue))
        {
            game.player.killCooldown = message.time;
        }
        else if(message.knife == true && message.target != playerMobId)
        {
            /*
            Debug.Log("KnifeKill");
            KnifeDieEvent de = new KnifeDieEvent();
            de.UnitGameObjectPos = game.player.transform.position;
            de.UnitParticle = game.knifeKillEffect;
         //   se.UnitSound = gameOverEvent.victory ? game.gameWinSounds : game.gameLostSounds;
            EventSystem.Current.FireEvent(EVENT_TYPE.KNIFE_KILL, de);
            */
        }
    }

    internal void ReportAttemptedHandler(IPEndPoint endpoint, ReportAttempted message)
    {
        Debug.Log("Report");
        MeetingEvent umei = new MeetingEvent();
        umei.game = game;
        umei.idOfInitiator = message.idOfInitiator;
        umei.idOfBody = message.target;
        if(message.idOfInitiator == playerMobId && game.pulseActive == true)
        {
            PulseCheckerEvidence pce = new PulseCheckerEvidence();
            pce.Time = (int)mobs[message.target].timeSpentDead;
            pce.player = mobs[message.idOfInitiator].sprite.sprite;
            pce.dead = mobs[message.target].sprite.sprite;
            pce.playerId = message.idOfInitiator;
            pce.deadId = message.target;
            pce.playerName = names[message.idOfInitiator];
            pce.deadName = names[message.target];
            game.eh.AddPulseCheckerEvidence(pce);
            game.pulseActive = false;
        }
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
            mob.SetColor(message.color.x, message.color.y, message.color.z);
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
        Reset();
        game.connectionState = GameController.ConnectionState.JoiningLobby;
        game.timeout = game.time + 2000000000;
        game.UpdateName();
    }

    internal void TaskListUpdateHandler(IPEndPoint endpoint, TaskListUpdate message)
    {
        game.taskManager.tasks.Clear();
        foreach (var task in message.tasks)
        {
            if (task > 0 && !game.taskManager.minigameInitiators.ContainsKey(task))
                game.CreateInfoPopup("Minigame index " + task + " not found.");
            game.taskManager.tasks.Add(new Task { minigame_index = task, completed = false });
        }
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
        se.UnitSound = gameOverEvent.victory ? game.gameWinSounds : game.gameLostSounds;
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
    }

    internal void PickupCooldownHandler(IPEndPoint endpoint, PickupCooldown message)
    {
        CooldownEvent cdEvent = new CooldownEvent();
        cdEvent.child = (int)message.child;
        cdEvent.random = message.random;
        EventSystem.Current.FireEvent(EVENT_TYPE.PICKUP_WAIT, cdEvent);
    }

    internal void TeleportToMeetingHandler(IPEndPoint endpoint, TeleportToMeeting message)
    {
    }

    internal void HideAttemptedHandler(IPEndPoint endpoint, HideAttempted message)
    {
        if (game.lockerManager.Lockers[message.index].occupied == true)
        {
            game.lockerManager.Lockers[message.index].occupied = false;
            game.lockerManager.Lockers[message.index].RemovePerson();
            if (message.user == playerMobId)
                game.player.canMove = true;
            if (!game.lockerManager.Lockers[message.index].GetComponentInChildren<AudioSource>().isPlaying)
                game.lockerManager.Lockers[message.index].GetComponentInChildren<AudioSource>().Play();
        }
        else if (game.lockerManager.Lockers[message.index].occupied == false)
        {
            game.lockerManager.Lockers[message.index].occupied = true;
            //game.lockerManager.Lockers[message.index].occupant = mobs[message.user].gameObject;
            if (message.user == playerMobId)
                game.player.canMove = false;
            game.lockerManager.Lockers[message.index].HidePerson(mobs[message.user].gameObject);
            if (!game.lockerManager.Lockers[message.index].GetComponentInChildren<AudioSource>().isPlaying)
                game.lockerManager.Lockers[message.index].GetComponentInChildren<AudioSource>().Play();
        }
    }

    internal void GetAllPlayerPositionsHandler(IPEndPoint endpoint, GetAllPlayerPositions message)
    {
    }

    internal void PhotoPoseHandler(IPEndPoint endpoint, PhotoPose message)
    {
    }

    internal void TakePhotoHandler(IPEndPoint endpoint, TakePhoto message)
    {
    }

    internal void PresentEvidenceHandler(IPEndPoint endpoint, PresentEvidence message)
    {
        PresentEvidenceEvent presentEvidenceEvent = new PresentEvidenceEvent();
        presentEvidenceEvent.index = (int)message.index;
        presentEvidenceEvent.presenter = message.presenter;
        EventSystem.Current.FireEvent(EVENT_TYPE.PRESENT_EVIDENCE, presentEvidenceEvent);
    }

    internal void SendSensorListHandler(IPEndPoint endpoint, SendSensorList message)
    {
        Debug.Log("SendSensor");
        SendEvidenceEvent seEvent = new SendEvidenceEvent();
        MotionSensor ms = new MotionSensor();
        
        string byteString = Encoding.UTF8.GetString(message.names.ToArray());
        List<string> dada = byteString.Split(';').ToList();
        foreach(string s in dada)
        {
            Debug.Log(s);
        }
        ms.names = dada;
        
        ms.secondsIn = message.times.Select(item => (int)item).ToList();

        List <Sprite> playerSprites = new List<Sprite>();
        foreach (ulong id in message.playerIds)
        {
            playerSprites.Add(mobs[id].sprite.sprite);
        }
        ms.playerSprites = playerSprites;
        seEvent.MotionSensorEvidence = ms;
        seEvent.Evidence = 2;
        EventSystem.Current.FireEvent(EVENT_TYPE.SEND_EVIDENCE, seEvent);
    }

    internal void SmokeGrenadeActivateHandler(IPEndPoint endpoint, SmokeGrenadeActivate message)
    {
        Debug.Log("Spawn Smoke");
        game.SpawnSmokeGrenade((Vector2)message.pos);
    }

    internal void PhotoTakenHandler(IPEndPoint endpoint, PhotoTaken message)
    {
        Photo photo = new Photo();
        photo.poses = message.poses;
        photo.photographer = message.photographer;
        game.screenshotHandler.photos.Add(message.index, photo);
    }
    internal void PulseEvidenceHandler(IPEndPoint endpoint, PulseEvidence message)
    {
        SendEvidenceEvent seEvent = new SendEvidenceEvent();
        PulseCheckerEvidence pce = new PulseCheckerEvidence();
        pce.playerName = names[message.playerId];
        pce.deadName = names[message.deadId];
        pce.player = mobs[message.playerId].sprite.sprite;
        pce.dead = mobs[message.deadId].sprite.sprite;
        pce.Time = message.deadTime;
        seEvent.pulseCheckerEvidence = pce;
        seEvent.Evidence = 4;
        EventSystem.Current.FireEvent(EVENT_TYPE.SEND_EVIDENCE, seEvent);
    }
    internal void SmokeGrenadeEvidenceHandler(IPEndPoint endpoint, SmokeGrenadeEvidence message)
    {
        Debug.Log("Smoke Evidence Info  AREA: " + message.area + " PlayerID: " + message.playerId);
        SendEvidenceEvent seEvent = new SendEvidenceEvent();
        SGEvidence sg = new SGEvidence();
        sg.area = message.area;
        sg.playerName = message.playerName;
        sg.player = mobs[message.playerId].sprite.sprite;
        seEvent.smokeGrenadeEvidence = sg;
        seEvent.Evidence = 3;
        EventSystem.Current.FireEvent(EVENT_TYPE.SEND_EVIDENCE, seEvent);
    }

}
