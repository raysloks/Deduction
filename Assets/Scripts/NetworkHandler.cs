using System.Net;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHandler
{

    public GameController controller;

    public Link link;

    public Dictionary<ulong, GameObject> mobs = new Dictionary<ulong, GameObject>();

    public NetworkHandler()
    {
        link = new Link();
        link.handler = this;
        link.Open(new IPEndPoint(IPAddress.Any, 0));
        link.Connect(new IPEndPoint(IPAddress.Parse("172.105.79.194"), 16343));
        link.Receive();
    }

    internal void MobUpdateHandler(IPEndPoint endpoint, MobUpdate message)
    {
        if (!mobs.ContainsKey(message.id))
            mobs.Add(message.id, Object.Instantiate(controller.prefab));
        mobs[message.id].transform.position = message.position;
    }

    internal void PlayerUpdateHandler(IPEndPoint endpoint, PlayerUpdate message)
    {
    }

    internal void HeartbeatHandler(IPEndPoint endpoint, Heartbeat message)
    {
        Debug.Log(message.time);
    }

}