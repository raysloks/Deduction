using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using EventCallbacks;
using System.Xml;
using System.Linq;
using System.IO;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    public GameSettings settings;

    public GameObject prefab;

    public MinigamePopupScript popup;

    public TaskManager taskManager;

    public DoorManager doorManager;

    private float heartbeat = 0f;
    private float snapshot = 0f;

    public Player player;

    public Text text;


    public InputField nameInputField;
    public InputField lobbyInputField;

    public GameObject connectionMenu;

    public GameObject setupMenu;
    public GameObject setupLeaderMenu;

    public GameObject hud;

    public DeathAnimation deathAnimation;


    public Slider taskbar;


    public Button killButton;
    public Text killCooldownText;

    public GameObject targetMarker;

    public GameObject noteLocation;


    public Button reportButton;


    public Button useButton;


    public float lightTarget = 1.0f;
    public float lightCurrent = 1.0f;


    public long time;
    public long timeout;
    public bool TimerOn => timer != 0;

    // public bool nearEmergencyButton = false;

    public NetworkHandler handler;
    public MatchmakerHandler matchmaker;
    public VoiceManager voice;

    public GameSettingsManager settingsManager;

    public List<AudioClip> gameWinSounds;
    public List<AudioClip> gameLostSounds;

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

    private Interactable useTarget = null;
    public Interactable UseTarget
    {
        get => useTarget;
        set
        {
            if (useTarget != value)
            {
                if (useTarget != null)
                    useTarget.Highlight(false);
                if (value != null)
                    value.Highlight(true);
                useTarget = value;
            }
        }
    }

    private bool isLeader = false;
    public bool IsLeader
    {
        get => isLeader;
        set
        {
            isLeader = value;
            settingsManager.SetInteractable(value);
            setupLeaderMenu.SetActive(value);
        }
    }

    private bool streamerMode = false;

    void Start()
    {
        handler = new NetworkHandler();
        handler.game = this;

        matchmaker = new MatchmakerHandler();
        matchmaker.controller = this;

        voice = new VoiceManager();
        voice.handler = handler;

        player.controller = this;
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

        lightCurrent = Mathf.MoveTowards(lightCurrent, lightTarget, Time.deltaTime);

        //if (Input.GetKeyDown(KeyCode.Alpha2) && phase == GamePhase.Main)
        //    handler.link.Send(new MeetingRequested());
        if (Input.GetKeyDown(KeyCode.F5) && phase != GamePhase.Setup)
            handler.link.Send(new RestartRequested());

        for (int i = 0; i < 10; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + (i + 1) % 10))
                Sabotage(i);
        }

        targetMarker.SetActive(false);
        killButton.gameObject.SetActive(player.role == 1 && player.IsAlive);
        reportButton.gameObject.SetActive(player.IsAlive);
        hud.gameObject.SetActive(phase == GamePhase.Main);
        taskbar.gameObject.SetActive(
            phase == GamePhase.Main || 
            phase == GamePhase.Discussion || 
            phase == GamePhase.Voting || 
            phase == GamePhase.EndOfMeeting);

        if (phase == GamePhase.Main && player.IsAlive)
        {
            // Kill
            if (player.role == 1)
            {
                killTarget = ulong.MaxValue;
                float targetDistance = settings.killRange;
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
        }

        if (phase == GamePhase.Main)
        {
            // LINQ should be removed for performance gains
            UseTarget = Physics2D.OverlapCircleAll(player.transform.position, 1.0f, 1 << 0)
                .Select(x => x.GetComponent<Interactable>())
                .Where(x => x != null && x.CanInteract(this))
                .OrderBy(x => Vector2.Distance(x.transform.position, player.transform.position))
                .FirstOrDefault();
            useButton.interactable = UseTarget != null && !popup.Active;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (popup.Active)
                    popup.DeactivatePopup();
                else
                    Use();
            }
        }

        connectionMenu.SetActive(connectionState == ConnectionState.None);
        setupMenu.SetActive(connectionState == ConnectionState.Connected && phase == GamePhase.Setup && timer == 0);

        text.fontSize = 36;
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
                int secondsRemaining = (int)((timer - time + 999999999) / 1000000000);
                text.fontSize = 72;
                switch (phase)
                {
                    case GamePhase.Setup:
                        if (timer != 0)
                            text.text = "Game starting in " + secondsRemaining;
                        else
                        {
                            if (streamerMode)
                            {
                                text.text = "Setup ";
                                for (int i = 0; i < matchmaker.lobby.Length; ++i)
                                    text.text += "*";
                            }
                            else
                            {
                                text.text = "Setup " + matchmaker.lobby;
                            }
                        }
                        break;
                    case GamePhase.Intro:
                        text.text = "";
                        break;
                    case GamePhase.Main:
                        text.text = "";
                        //text.text = (Time.deltaTime * 1000f).ToString("F1");
                        break;
                    case GamePhase.Discussion:
                        text.text = "Voting begins in " + secondsRemaining;
                        break;
                    case GamePhase.Voting:
                        text.text = "Voting ends in " + secondsRemaining;
                        if (secondsRemaining <= 10)
                            text.fontSize = 120 - secondsRemaining * 3;
                        break;
                    case GamePhase.EndOfMeeting:
                        text.text = "Meeting ends in " + secondsRemaining;
                        break;
                    case GamePhase.Ejection:
                        text.text = "";
                        long dots = 5 - secondsRemaining;
                        for (int i = 0; i < dots; ++i)
                            text.text += ".";
                        break;
                    case GamePhase.GameOver:
                        text.fontSize = 36;
                        text.text = "Setup in " + secondsRemaining;
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
        popup.DeactivatePopup();

        if (phase == GamePhase.Setup || phase == GamePhase.GameOver)
        {
            taskManager.tasks.Clear();
            lightTarget = 1.0f;
            lightCurrent = 1.0f;
        }

        if (phase == GamePhase.Main && previous == GamePhase.Intro)
            player.emergencyButtonsLeft = (int)settings.emergencyMeetingsPerPlayer;

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

    public void Use()
    {
        if (UseTarget != null)
        {
            UseTarget.Interact(this);
        }
    }

    public void Sabotage(int index)
    {
        handler.link.Send(new AbilityUsed { ability = (ulong)index });
    }

    public void StartGame()
    {
        if (phase == GamePhase.Setup)
        {
            int count = noteLocation.transform.childCount;
            string pass = readTextFile("Passwords.txt");
            for (int i = 0; i < 4; i++)
            {
                int r = UnityEngine.Random.Range(0, 9);
                pass += r.ToString();
            }


            GameStartRequested message = new GameStartRequested
            {
                password = pass,
                passwordLocation = (ulong)UnityEngine.Random.Range(0, (count - 1))
            };
            handler.link.Send(message);
        }
    } 

    private string readTextFile(string fileName)
    {
        string result;
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/" + fileName);
        string fileContents = sr.ReadToEnd();
        sr.Close();
        result = fileContents;

        string[] lines = result.Split("\n"[0]);

        int r = UnityEngine.Random.Range(0, lines.Length - 1);
        string str = lines[r];
        if (str != null && str.Length > 0)
        {
            str = str.Substring(0, str.Length - 1);
        }
        return str;
    }

    public void UpdateName()
    {
        string name = nameInputField.text;
        name = name.Trim();
        if (name.Length == 0)
            name = "Agent " + rng.RangeInt(1, 1000).ToString().PadLeft(3, '0');
        handler.link.Send(new PlayerUpdate { name = name });
    }

    public void ResetSettings()
    {
        if (phase == GamePhase.Setup)
            handler.link.Send(new ResetGameSettings());
    }

    public void SetStreamerMode(bool value)
    {
        if (streamerMode != value)
        {
            streamerMode = value;
            lobbyInputField.contentType = value ? InputField.ContentType.Password : InputField.ContentType.Standard;
        }
    }

}
