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

    public InputField nameInputField;
    public InputField lobbyInputField;

    public GameObject connectionMenu;

    public long time;
    public long timeout;
    public bool timerOn = true;


    public NetworkHandler handler;
    public MatchmakerHandler matchmaker;
    public VoiceManager voice;

    public bool listenToSelf = false;


    public Xoroshiro128Plus rng = new Xoroshiro128Plus();

    public enum ConnectionState
    {
        None,
        ConnectingToMatchmaker,
        RequestingLobby,
        ConnectingToLobby,
        JoiningLobby,
        Connected
    }

    public ConnectionState connectionState;

    void Start()
    {
        handler = new NetworkHandler();
        handler.controller = this;

        matchmaker = new MatchmakerHandler();
        matchmaker.controller = this;

        voice = new VoiceManager();
        voice.handler = handler;
    }

    private void Update()
    {
        voice.Stream();

        handler.link.Poll();
        matchmaker.link.Poll();

        time += (long)(Time.deltaTime * 1000000000);

        if (time > timeout && timeout != 0)
        {
            connectionState = ConnectionState.None;
            phase = GamePhase.None;
            timeout = 0;
        }

        heartbeat -= Time.deltaTime;
        if (heartbeat <= 0f)
        {
            handler.link.Send(new Heartbeat());
            heartbeat += 1f;
        }

        snapshot -= Time.deltaTime;
        if (snapshot <= 0f)
        {
            if (connectionState == ConnectionState.JoiningLobby)
            {
                string name = nameInputField.text;
                name = name.Trim();
                if (name.Length == 0)
                    name = "Agent " + rng.RangeInt(1, 1000).ToString().PadLeft(3, '0');
                handler.link.Send(new PlayerUpdate { name = name });
            }
            handler.link.Send(new MobUpdate { position = player.transform.position });
            snapshot += 0.05f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            handler.link.Send(new GameStartRequested());
        if (Input.GetKeyDown(KeyCode.Alpha2))
            handler.link.Send(new MeetingRequested());
        if (Input.GetKeyDown(KeyCode.Alpha3))
            handler.link.Send(new RestartRequested());

        if (Input.GetKeyDown(KeyCode.Q) && phase == GamePhase.Main)
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
                        break;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && phase == GamePhase.Main)
        {
            foreach (var n in handler.mobs)
            {
                if(n.Value.IsAlive == false)
                {
                    float distance = Vector2.Distance(player.transform.position, n.Value.transform.position);
                    if(distance < reportDistance)
                    {
                        ReportAttempted message = new ReportAttempted
                        {
                            target = n.Key,
                            time = time
                        };
                        handler.link.Send(message);
                        break;
                    }
                }
            }

        }

        if (Input.GetMouseButtonDown(0) && phase == GamePhase.Meeting && timerOn == true)
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.CompareTag("VoteButton"))
            {
                PlayerVoted message = new PlayerVoted
                {
                    timer = timer,
                    totalVotes = totalAmountOfVotes,
                    buttonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name
                 };
                handler.link.Send(message);
            }
        }

        connectionMenu.SetActive(connectionState == ConnectionState.None);

        switch (connectionState)
        {
            case ConnectionState.None:
                text.text = "";
                break;
            case ConnectionState.ConnectingToMatchmaker:
                text.text = "Connecting to matchmaker...";
                break;
            case ConnectionState.RequestingLobby:
                text.text = "Requesting lobby...";
                break;
            case ConnectionState.ConnectingToLobby:
                text.text = "Connecting to lobby...";
                break;
            case ConnectionState.JoiningLobby:
                text.text = "Joining lobby...";
                break;
            case ConnectionState.Connected:
                switch (phase)
                {
                    case GamePhase.Setup:
                        if (timer != 0)
                            text.text = "Game starting in " + (timer - time + 999999999) / 1000000000;
                        else
                            text.text = "Setup " + matchmaker.lobby;
                        break;
                    case GamePhase.Main:
                        text.text = "";
                        break;
                    case GamePhase.Meeting:
                        if(timerOn == false)
                        {
                            text.text = "Meeting Ending";
                        }
                        if (((timer - time + 999999999) / 1000000000) > 0 && timerOn == true)
                        {
                            text.text = "Meeting " + (timer - time + 999999999) / 1000000000;
                        }
                        else if(timerOn == true)
                        {
                            timerOn = false;
                            MeetingEvent me = new MeetingEvent();
                            EventSystem.Current.FireEvent(EVENT_TYPE.MEETING_ENDED, me);
                        }
                        break;
                    case GamePhase.None:
                        text.text = "Waiting for server...";
                        break;
                }
                break;
        }
    }


    public void Connect()
    {
        matchmaker.ConnectToLobby(lobbyInputField.text);
        timeout = time + 20000000000;
    }

    public enum GamePhase
    {
        Setup,
        Main,
        Meeting,
        None
    }

    public GamePhase phase = GamePhase.None;
    public long timer;

    public void SetGamePhase(GamePhase phase, long timer)
    {
        if(phase == GamePhase.Meeting)
        {
            Debug.Log("Meeting Started");
          //  MeetingUi();
        }
        this.phase = phase;
        this.timer = timer;
    }

}
