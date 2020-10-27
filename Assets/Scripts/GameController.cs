using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using EventCallbacks;
using System.Xml;

public class GameController : MonoBehaviour
{
    public GameSettings settings;

    public GameObject prefab;

    public MinigamePopupScript popup;
    public List<MinigameInitiator> MinigameInitiators;

    public TaskManager taskManager;

    private float heartbeat = 0f;
    private float snapshot = 0f;

    public Player player;

    public Text text;

    public InputField nameInputField;
    public InputField lobbyInputField;

    public GameObject connectionMenu;

    public GameObject copyCodeButton;
    public Button startGameButton;

    public Button killButton;
    public Text killCooldownText;

    public Button reportButton;

    public GameObject targetMarker;

    public long time;
    public long timeout;
    public bool TimerOn => timer != 0;
	
    // public bool nearEmergencyButton = false;

    public NetworkHandler handler;
    public MatchmakerHandler matchmaker;
    public VoiceManager voice;

    public GameSettingsManager settingsManager;

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

    public GamePhase phase = GamePhase.None;
    public long timer;

    private ulong killTarget = ulong.MaxValue;
    private ulong reportTarget = ulong.MaxValue;

    void Start()
    {
        handler = new NetworkHandler();
        handler.game = this;

        matchmaker = new MatchmakerHandler();
        matchmaker.controller = this;

        voice = new VoiceManager();
        voice.handler = handler;

        player.controller = this;

        killButton.onClick.AddListener(Kill);
        reportButton.onClick.AddListener(Report);
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

        //if (Input.GetKeyDown(KeyCode.Alpha2) && phase == GamePhase.Main)
        //    handler.link.Send(new MeetingRequested());
        if (Input.GetKeyDown(KeyCode.Escape) && phase != GamePhase.Setup)
            handler.link.Send(new RestartRequested());

        targetMarker.SetActive(false);
        killButton.gameObject.SetActive(player.role == 1 && phase == GamePhase.Main && player.isPlayerAlive());
        reportButton.gameObject.SetActive(phase == GamePhase.Main && player.isPlayerAlive());

        if (phase == GamePhase.Main && player.isPlayerAlive())
        {
            // Kill
            if (player.role == 1)
            {
                killTarget = ulong.MaxValue;
                float targetDistance = 1.75f;
                if (player.killCooldown < time)
                {
                    foreach (var n in handler.mobs)
                    {
                        if (n.Value.IsAlive == true && n.Value.role == 0 && n.Value.gameObject.activeSelf)
                        {
                            float distance = Vector2.Distance(player.transform.position, n.Value.transform.position);
                            if (distance < targetDistance)
                            {
                                killTarget = n.Key;
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
                killButton.interactable = killTarget != ulong.MaxValue;
                if (killTarget != ulong.MaxValue)
                {
                    targetMarker.SetActive(true);
                    targetMarker.transform.position = handler.mobs[killTarget].transform.position;
                    if (Input.GetKeyDown(KeyCode.Q))
                        Kill();
                }
		    }


            // Report
            {
                reportTarget = ulong.MaxValue;
                float targetDistance = player.GetVision();
                foreach (var n in handler.mobs)
                {
                    if (n.Value.IsAlive == false && n.Value.gameObject.activeSelf)
                    {
                        Vector2 diff = n.Value.transform.position - player.transform.position;
                        float distance = diff.magnitude;
                        if (distance < targetDistance)
                        {
                            if (!Physics2D.Raycast(player.transform.position, diff / distance, distance, 1 << 10))
                            {
                                reportTarget = n.Key;
                                targetDistance = distance;
                            }
                        }
                    }
                }
                reportButton.interactable = reportTarget != ulong.MaxValue;
                if (Input.GetKeyDown(KeyCode.R))
                    Report();
            }


            // Emergency Meeting
            if (Input.GetKeyDown(KeyCode.Space) && player.canRequestMeeting)
            {
                handler.link.Send(new MeetingRequested());
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            float targetDistance = (player.GetVision()) / 2;
            for (int i = 0; i < taskManager.tasks.Count; ++i)
            {
                MinigameInitiator minigameInitiator = MinigameInitiators[taskManager.tasks[i].index % MinigameInitiators.Count];
                if (minigameInitiator.isSolved == false)
                {
                    Vector2 diff = minigameInitiator.transform.position - player.transform.position;
                    float distance = diff.magnitude;
                    if (distance < targetDistance)
                    {
                        minigameInitiator.StartMinigame(i, this);
                        player.canMove = false;
                    }
                }
            }
            //popup.ActivatePopup("FredrikMinigame2", ini);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            popup.DeactivatePopup();
        }

        connectionMenu.SetActive(connectionState == ConnectionState.None);
        copyCodeButton.SetActive(connectionState == ConnectionState.Connected && phase == GamePhase.Setup && timer == 0);
        startGameButton.gameObject.SetActive(connectionState == ConnectionState.Connected && phase == GamePhase.Setup && timer == 0);

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
                    case GamePhase.Discussion:
                        text.text = "Voting begins in " + (timer - time + 999999999) / 1000000000;
                        break;
                    case GamePhase.Voting:
                        text.text = "Voting ends in " + (timer - time + 999999999) / 1000000000;
                        break;
                    case GamePhase.EndOfMeeting:
                        text.text = "Meeting ends in " + (timer - time + 999999999) / 1000000000;
                        break;
                    case GamePhase.Ejection:
                        text.text = "Ejecting";
                        long dots = 5 - (timer - time + 999999999) / 1000000000;
                        for (int i = 0; i < dots; ++i)
                            text.text += ".";
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

    public void SetGamePhase(GamePhase phase, long timer, GamePhase previous)
    {
        if (phase == GamePhase.Main && previous == GamePhase.Setup)
        {
            player.emergencyButtonsLeft = (int)settings.emergencyMeetingsPerPlayer;
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        player.canMove = phase == GamePhase.Setup || phase == GamePhase.Main || phase == GamePhase.GameOver;
        PhaseChangedEvent ee = new PhaseChangedEvent
        {
            phase = phase,
            timer = timer,
            previous = previous
        };
        EventSystem.Current.FireEvent(EVENT_TYPE.PHASE_CHANGED, ee);
        this.phase = phase;
        this.timer = timer;
    }

    public void Kill()
    {
        if (killTarget != ulong.MaxValue)
        {
            KillAttempted message = new KillAttempted
            {
                target = killTarget,
                time = time
            };
            handler.link.Send(message);
        }
    }

    public void Report()
    {
        if (reportTarget != ulong.MaxValue)
        {
            ReportAttempted message = new ReportAttempted
            {
                target = reportTarget,
                time = time
            };
            handler.link.Send(message);
        }
    }

    public void StartGame()
    {
        if (phase == GamePhase.Setup)
            handler.link.Send(new GameStartRequested());
    }

    public void ResetSettings()
    {
        if (phase == GamePhase.Setup)
            handler.link.Send(new ResetGameSettings());
    }

}
