using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public GameObject prefab;

    public NetworkHandler handler;

    private float heartbeat = 0f;

    private Vector3 position;

    void Start()
    {
        handler = new NetworkHandler();
        handler.controller = this;
    }

    private void Update()
    {
        handler.link.Poll();

        heartbeat -= Time.deltaTime;
        if (heartbeat <= 0f)
        {
            handler.link.Send(new Heartbeat());
            heartbeat = 1f;
        }

        handler.link.Send(new PlayerUpdate { name = "Astronaut" });
        handler.link.Send(new MobUpdate { position = position });
    }

}
