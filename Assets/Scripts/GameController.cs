using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using System;

public class GameController : MonoBehaviour
{
    public GameObject prefab;

    public NetworkHandler handler;

    private float heartbeat = 0f;
    private float snapshot = 0f;

    private Player player;

    void Start()
    {
        handler = new NetworkHandler();
        handler.controller = this;

        player = FindObjectOfType<Player>(); // todo fix
    }

    private void Update()
    {
        handler.link.Poll();

        heartbeat -= Time.deltaTime;
        if (heartbeat <= 0f)
        {
            handler.link.Send(new Heartbeat());
            heartbeat += 1f;
        }

        snapshot -= Time.deltaTime;
        if (snapshot <= 0f)
        {
            handler.link.Send(new PlayerUpdate { name = Environment.UserName });
            handler.link.Send(new MobUpdate { position = player.transform.position });
            snapshot += 0.05f;
        }
    }

}
