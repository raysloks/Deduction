using System.Net;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using EventCallbacks;

public class NetworkHandler
{
    public GameController controller;

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
        TextMeshProUGUI text = null;
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
                mobs.Add(message.id, UnityEngine.Object.Instantiate(controller.prefab, message.position, Quaternion.identity).GetComponent<NetworkMob>());
                UpdateName(message.id);
            }
            if (mobs[message.id] is NetworkMob mob)
                mob.AddSnapshot(new NetworkMob.Snapshot { time = message.time, position = message.position });
        }
    }

    internal void PlayerUpdateHandler(IPEndPoint endpoint, PlayerUpdate message)
    {
        if (message.id == ulong.MaxValue)
        {
            playerMobId = message.mob;
            mobs[playerMobId] = controller.player;
            controller.connectionState = GameController.ConnectionState.Connected;
        }
        else
            names[message.mob] = message.name;
        UpdateName(message.mob);
    }

    internal void HeartbeatHandler(IPEndPoint endpoint, Heartbeat message)
    {
        if (Math.Abs(controller.time - message.time) > 50000000)
            controller.time = message.time;
        controller.timeout = message.time + 5000000000;
    }

    internal void MobTeleportHandler(IPEndPoint endpoint, MobTeleport message)
    {
        if (message.id == playerMobId)
            controller.player.transform.position = message.to;
    }

    internal void GamePhaseUpdateHandler(IPEndPoint endpoint, GamePhaseUpdate message)
    {      
        controller.SetGamePhase((GameController.GamePhase)message.phase, message.timer);
    }

    internal void MeetingRequestedHandler(IPEndPoint endpoint, MeetingRequested message)
    {
        MeetingEvent umei = new MeetingEvent();
        umei.meetingHandler = this;
        umei.idOfInitiator = message.idOfInitiator;
        if(message.EmergencyMeetings == (ulong)controller.totalAmountOfMeetings)
        {
            umei.emergencyMeetingDone = true;
            if(message.idOfInitiator == playerMobId)
            {
                controller.player.emergencyButtonLeft = false;
                Debug.Log("ye this was the player" + message.idOfInitiator + "ye " + playerMobId);
            }
        }
        else
        {
            umei.emergencyMeetingDone = false;
        }
        umei.EventDescription = "Meeting Got Started";
        EventSystem.Current.FireEvent(EVENT_TYPE.MEETING_STARTED, umei);
    }

    internal void PlayerVotedHandler(IPEndPoint endpoint, PlayerVoted message)
    {
        Debug.Log("This is the phase " + message.phase + " this player voted " + names[message.id]);


        VoteEvent uvei = new VoteEvent();

        if (message.votesLeft == 0)
        {
            uvei.doneVoting = true;
            Debug.Log("Done Voting");
        }
        else
        {
            uvei.doneVoting = false;
        }

        uvei.idOfVoter = message.id;
        uvei.EventDescription = "Player Voted";
        uvei.totalAmountOfVotes = (int)message.totalVotes;
        uvei.nameOfButton = message.buttonName;
        EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.MEETING_VOTED, uvei);

    }

    internal void GameStartRequestedHandler(IPEndPoint endpoint, GameStartRequested message)
    {
        controller.ApplySettings();
    }
    internal void RestartRequestedHandler(IPEndPoint endpoint, RestartRequested message)
    {
    }

    internal void MobRemovedHandler(IPEndPoint endpoint, MobRemoved message)
    {
        if (mobs.ContainsKey(message.id) && message.id != playerMobId)
        {
            UnityEngine.Object.Destroy(mobs[message.id].gameObject);
            mobs.Remove(message.id);
        }
        removalTimes[message.id] = message.time;
    }

    internal void KillAttemptedHandler(IPEndPoint endpoint, KillAttempted message)
    {
        controller.player.killCooldown = message.time;
    }

    internal void ReportAttemptedHandler(IPEndPoint endpoint, ReportAttempted message)
    {
        MeetingEvent umei = new MeetingEvent();
        umei.meetingHandler = this;
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
        if (mobs.ContainsKey(message.id) && (message.id != playerMobId || controller.listenToSelf))
            voicePlayer = mobs[message.id].GetComponentInChildren<VoicePlayer>();
        if (voicePlayer)
            voicePlayer.ProcessFrame(message.data);
    }

    internal void ConnectionHandler(IPEndPoint endpoint)
    {
        controller.connectionState = GameController.ConnectionState.JoiningLobby;

        string name = controller.nameInputField.text;
        name = name.Trim();
        if (name.Length == 0)
            name = "Agent " + controller.rng.RangeInt(1, 1000).ToString().PadLeft(3, '0');
        link.Send(new PlayerUpdate { name = name });
    }

    internal void TaskListUpdateHandler(IPEndPoint endpoint, TaskListUpdate message)
    {
    }

    internal void TaskUpdateHandler(IPEndPoint endpoint, TaskUpdate message)
    {
    }

    internal void GameSettingSetHandler(IPEndPoint endpoint, GameSettingSet message)
    {
        controller.settings.SetSetting(message.setting, message.value);
    }

    internal void GameSettingsUpdateHandler(IPEndPoint endpoint, GameSettingsUpdate message)
    {
    }
}