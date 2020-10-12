using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using EventCallbacks;

public class GameController : MonoBehaviour
{
    public GameObject prefab;

    public float reportDistance = 4f;
    public int totalAmountOfVotes = 2;

    private float heartbeat = 0f;
    private float snapshot = 0f;

    public Player player;

    public Text text;

    public long time;
    public long timeout;

    public NetworkHandler handler;
    public VoiceManager voice;

    public bool listenToSelf = false;

    void Start()
    {
        handler = new NetworkHandler();
        handler.controller = this;

        phase = GamePhase.None;

        if (!player)
            player = FindObjectOfType<Player>(); // todo fix

        voice = new VoiceManager();
        voice.handler = handler;
    }

    private void Update()
    {
        voice.Stream();

        handler.link.Poll();

        time += (long)(Time.deltaTime * 1000000000);

        if (time > timeout && timeout != 0)
            phase = GamePhase.None;

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
            handler.link.Send(new GameStartRequested());
        if (Input.GetKeyDown(KeyCode.Alpha2))
            handler.link.Send(new MeetingRequested());
        if (Input.GetKeyDown(KeyCode.Alpha3))
            handler.link.Send(new RestartRequested());

        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (var n in handler.mobs)
            {
                if (n.Value.IsAlive == true)
                {
                    float distance = Vector2.Distance(player.transform.position, n.Value.transform.position);
                    if (distance < 2.25f)
                    {
                        KillAttempted message = new KillAttempted
                        {
                            target = n.Key,
                            time = time
                        };
                        handler.link.Send(message);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (NetworkMob n in handler.mobs.Values)
            {
                if(n.IsAlive == false)
                {
                    float distance = Vector2.Distance(player.transform.position, n.transform.position);
                    if(distance < reportDistance)
                    {

                        ReportAttempted message = new ReportAttempted
                        {
                            target = 0,
                            time = time
                        };
                        handler.link.Send(message);
                    }
                }
            }

        }

        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.CompareTag("VoteButton"))
            {
                PlayerVoted message = new PlayerVoted
                {                    
                    timer = timer,
                    totalVotes = totalAmountOfVotes
                 };
                handler.link.Send(message);
                
            }
        }

        switch (phase)
        {
            case GamePhase.Setup:
                if (timer != 0)
                    text.text = "Game starting in " + (timer - time + 999999999) / 1000000000;
                else
                    text.text = "Setup";
                break;
            case GamePhase.Main:
                text.text = "";
                break;
            case GamePhase.Meeting:
                if (timer != 0)
                {
                    text.text = "Meeting " + (timer - time + 999999999) / 1000000000;
                } break;
            case GamePhase.None:
                text.text = "Connecting...";
                break;
        }
    }

    public void MeetingUi()
    {
        MeetingEvent umei = new MeetingEvent();
        umei.meetingHandler = this.handler;
        umei.EventDescription = "Meeting Got Started";

        EventSystem.Current.FireEvent(EVENT_TYPE.MEETING_STARTED, umei);
    }

    public enum GamePhase
    {
        Setup,
        Main,
        Meeting,
        None
    }

    public GamePhase phase;
    public long timer;

    public void SetGamePhase(GamePhase phase, long timer)
    {
        if(phase == GamePhase.Meeting)
        {
            Debug.Log("Meeting Started");
            MeetingUi();
        }
        this.phase = phase;
        this.timer = timer;
    }

}
