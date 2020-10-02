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

        Vector3 move = new Vector3();
        if (Input.GetKey(KeyCode.W))
            move += Vector3.up;
        if (Input.GetKey(KeyCode.A))
            move += Vector3.left;
        if (Input.GetKey(KeyCode.S))
            move += Vector3.down;
        if (Input.GetKey(KeyCode.D))
            move += Vector3.right;
        move = Vector3.ClampMagnitude(move, 1f);
        position += move * Time.deltaTime * 5f;
    }

}
