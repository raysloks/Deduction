using System.Net;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class NetworkHandler
{

    public GameController controller;

    public Link link;

    public Dictionary<ulong, NetworkMob> mobs = new Dictionary<ulong, NetworkMob>();
    public Dictionary<ulong, string> names = new Dictionary<ulong, string>();
    public Dictionary<ulong, ulong> types = new Dictionary<ulong, ulong>();

    public ulong player_mob_id = ulong.MaxValue;

    public NetworkHandler()
    {
        link = new Link();
        link.handler = this;
        link.Open(new IPEndPoint(IPAddress.Any, 0));
        link.Connect(new IPEndPoint(IPAddress.Parse("172.105.79.194"), 16343));
        link.Receive();
    }

    private void UpdateName(ulong mob)
    {
        if (mobs.ContainsKey(mob) && names.ContainsKey(mob))
            mobs[mob].GetComponentInChildren<TextMeshProUGUI>().text = names[mob];
    }

    private void UpdateState(ulong mob)
    {
        if (mobs.ContainsKey(mob) && types.ContainsKey(mob))
            mobs[mob].SetType(types[mob]);
    }

    internal void MobUpdateHandler(IPEndPoint endpoint, MobUpdate message)
    {
        if (message.id != player_mob_id)
        {
            if (!mobs.ContainsKey(message.id))
            {
                mobs.Add(message.id, UnityEngine.Object.Instantiate(controller.prefab, message.position, Quaternion.identity).GetComponent<NetworkMob>());
                UpdateName(message.id);
            }
            mobs[message.id].AddSnapshot(new NetworkMob.Snapshot { time = message.time, position = message.position });
        }
    }

    internal void PlayerUpdateHandler(IPEndPoint endpoint, PlayerUpdate message)
    {
        if (message.id == ulong.MaxValue)
            player_mob_id = message.mob;
        else
        {
            names[message.mob] = message.name;
            UpdateName(message.mob);
        }
    }

    internal void HeartbeatHandler(IPEndPoint endpoint, Heartbeat message)
    {
        controller.time = message.time;
        controller.timeout = message.time + 5000000000;
    }

    internal void MobTeleportHandler(IPEndPoint endpoint, MobTeleport message)
    {
        if (message.id == player_mob_id)
            controller.player.transform.position = message.to;
    }

    internal void GamePhaseUpdateHandler(IPEndPoint endpoint, GamePhaseUpdate message)
    {
        controller.SetGamePhase((GameController.GamePhase)message.phase, message.timer);
    }

    internal void MeetingRequestedHandler(IPEndPoint endpoint, MeetingRequested message)
    {
    }

    internal void PlayerVotedHandler(IPEndPoint endpoint, PlayerVoted message)
    {
    }

    internal void GameStartRequestedHandler(IPEndPoint endpoint, GameStartRequested message)
    {
    }

    internal void RestartRequestedHandler(IPEndPoint endpoint, RestartRequested message)
    {
    }

    internal void MobRemovedHandler(IPEndPoint endpoint, MobRemoved message)
    {
        if (mobs.ContainsKey(message.id))
        {
            UnityEngine.Object.Destroy(mobs[message.id].gameObject);
            mobs.Remove(message.id);
        }
    }

    internal void KillAttemptedHandler(IPEndPoint endpoint, KillAttempted message)
    {
    }

    internal void ReportAttemptedHandler(IPEndPoint endpoint, ReportAttempted message)
    {
    }

    internal void AbilityUsedHandler(IPEndPoint endpoint, AbilityUsed message)
    {
    }

    internal void MobRoleUpdateHandler(IPEndPoint endpoint, MobRoleUpdate message)
    {
    }

    internal void MobStateUpdateHandler(IPEndPoint endpoint, MobStateUpdate message)
    {
        MobUpdateHandler(endpoint, message.update);
        types[message.update.id] = message.type;
        UpdateState(message.update.id);
    }
}