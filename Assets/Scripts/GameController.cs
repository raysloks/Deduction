﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using EventCallbacks;
using System.Xml;

public class GameController : MonoBehaviour
{
    public GameObject prefab;

    public float reportDistance = 4f;
    public int totalAmountOfVotes = 2;
    public int totalAmountOfMeetings = 1;

    private float heartbeat = 0f;
    private float snapshot = 0f;

    public Player player;

    public Text text;

    public InputField nameInputField;
    public InputField lobbyInputField;

    public GameObject connectionMenu;

    public Button killButton;
    public Text killCooldownText;

    public Button reportButton;

    public GameObject targetMarker;

    public long time;
    public long timeout;
    public bool timerOn = true;
   // public bool nearEmergencyButton = false;

    public NetworkHandler handler;
    public MatchmakerHandler matchmaker;
    public VoiceManager voice;
    public GameSettings settings;

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
            if (connectionState == ConnectionState.Connected)
                handler.link.Send(new MobUpdate { position = player.transform.position, time = time });
            snapshot += 0.05f;
        }

        if (phase == GamePhase.Setup)
            if (Input.GetKeyDown(KeyCode.Space))
                handler.link.Send(new GameStartRequested());
        if (Input.GetKeyDown(KeyCode.Alpha2) && phase == GamePhase.Main)
            handler.link.Send(new MeetingRequested());
        if (phase != GamePhase.Setup)
            if (Input.GetKeyDown(KeyCode.Escape))
                handler.link.Send(new RestartRequested());

        targetMarker.SetActive(false);
        killButton.gameObject.SetActive(player.role == 1 && phase == GamePhase.Main);

        reportButton.gameObject.SetActive(phase == GamePhase.Main);

        if (player.role == 1 && phase == GamePhase.Main)
        {
                ulong target = ulong.MaxValue;
                float targetDistance = 1.75f;
                if (player.killCooldown < time)
                {
                    foreach (var n in handler.mobs)
                    {
                        if (n.Value.IsAlive == true && n.Value.role == 0)
                        {
                            float distance = Vector2.Distance(player.transform.position, n.Value.transform.position);
                            if (distance < targetDistance)
                            {
                                target = n.Key;
                                targetDistance = distance;
                            }
                        }
                    }
                    killCooldownText.text = "";
                }
                else
                {
                    killCooldownText.text = ((player.killCooldown - time + 999999999) / 1000000000).ToString();
                }
                killButton.interactable = target != ulong.MaxValue;
                if (target != ulong.MaxValue)
                {
                    targetMarker.SetActive(true);
                    targetMarker.transform.position = handler.mobs[target].transform.position;
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        KillAttempted message = new KillAttempted
                        {
                            target = target,
                            time = time
                        };
                        handler.link.Send(message);
                    }
                }
            
        }

        if (Input.GetKeyDown(KeyCode.Space) && phase == GamePhase.Main && player.nearEmergencyButton)
        {
            handler.link.Send(new MeetingRequested());
        }

        if (Input.GetKeyDown(KeyCode.R) && phase == GamePhase.Main)
        {
                ulong target = ulong.MaxValue;
                float targetDistance = reportDistance;
                foreach (var n in handler.mobs)
                {
                    if (n.Value.IsAlive == false)
                    {
                        Vector2 diff = n.Value.transform.position - player.transform.position;
                        float distance = diff.magnitude;
                        if (distance < targetDistance)
                        {
                            if (!Physics2D.Raycast(player.transform.position, diff / distance, distance))
                            {
                                target = n.Key;
                                targetDistance = distance;
                            }
                        }
                    }
                }
                reportButton.interactable = target != ulong.MaxValue;
                if (target != ulong.MaxValue)
                {
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        ReportAttempted message = new ReportAttempted
                        {
                            target = target,
                            time = time
                        };
                        handler.link.Send(message);
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

        if (Input.GetKeyDown(KeyCode.Space) && phase == GamePhase.Main && player.nearEmergencyButton)
        {
            MeetingRequested message = new MeetingRequested
            {
                EmergencyMeetings = (ulong)totalAmountOfMeetings
            };
            handler.link.Send(message);
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
        player.cantMove = (phase == GamePhase.Meeting);
        if(phase == GamePhase.Main)
        {
            DebugEvent ee = new DebugEvent();
            ee.EventDescription = "Reset Emergency Button Timer";
            EventSystem.Current.FireEvent(EVENT_TYPE.RESET_TIMER, ee);
        }
        this.phase = phase;
        this.timer = timer;
    }

    public void ApplySettings()
    {
        totalAmountOfVotes = (int)settings.GetSetting("Votes Per Player").value;
        totalAmountOfMeetings = (int)settings.GetSetting("Emergency Meetings Per Player").value;
        SettingEvent se = new SettingEvent();
        se.settings = settings;
        EventSystem.Current.FireEvent(EVENT_TYPE.SETTINGS, se);
    }

}
