using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using EventCallbacks;
using System.Xml;
using System.Linq;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameSettings settings;

    public GameObject prefab;

    public MinigamePopupScript popup;

    public TaskManager taskManager;

    public DoorManager doorManager;

    public MapManager mapManager;

    public ScreenshotHandler screenshotHandler;

    public LockerManager lockerManager;

    public Transform mobContainer;

    private float heartbeat = 0f;
    private float snapshot = 0f;

    public Player player;

    public Text text;

    public GameObject infoPopupPrefab;
    public GameObject confirmPopupPrefab;

    private GameObject currentConfirmPopup;


    public InputField nameInputField;
    public InputField lobbyInputField;

    public GameObject connectionMenu;

    public GameObject setupMenu;
    public GameObject setupLeaderMenu;

    public Canvas canvas;

    public GameObject hud;

    public DeathAnimation deathAnimation;


    public Slider taskbar;


    public Button killButton;
    public Text killCooldownText;

    public TextMeshProUGUI areaText;

    public GameObject targetMarker;

    public GameObject noteLocation;

    public GameObject smokeGrenadePrefab;

    public Light2D globalLight;
    public bool darknessOnPlay;

    public Button reportButton;

    public Button itemButton;


    public Button useButton;

    [HideInInspector] public bool knifeItem = false;
    public GameObject knifeKillEffect;

    [HideInInspector] public bool pulseActive = false;

    public EvidenceHandler eh;

    public ColorPicker colorPicker;

    public float lightTarget = 1.0f;
    public float lightCurrent = 1.0f;


    public long time;
    public long timeout;
    public bool TimerOn => timer != 0;
    public float roundTimer = 0f;

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

    public GameObject accessibilityToggle;
    private bool showAccessibilityToggle = false;

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
        matchmaker.game = this;

        voice = new VoiceManager();
        voice.handler = handler;

        player.controller = this;

        //Easier level editing
        canvas.gameObject.SetActive(true);
        if (darknessOnPlay == true) globalLight.intensity = 0.2f;
    }

    private void Update()
    {
        voice.Stream();

        handler.link.Poll();
        matchmaker.link.Poll();

        time += (long)(Time.deltaTime * 1000000000);
        roundTimer += Time.deltaTime;

        if (time > timeout && timeout != 0)
        {
            switch (connectionState)
            {
                case ConnectionState.ConnectingToLobby:
                    if (matchmaker.lobby.Length == 0)
                    {
                        matchmaker.ConnectToLobby(matchmaker.lobby);
                        break;
                    }
                    goto default;
                default:
                    connectionState = ConnectionState.None;
                    phase = GamePhase.None;
                    timeout = 0;
                    CreateInfoPopup("Connection timed out.");
                    break;
            }
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
                handler.link.Send(new MobUpdate { position = player.transform.position, time = time, flipped = player.characterTransform.localScale.x < 0f });
            snapshot += 0.05f;
        }

        lightCurrent = Mathf.MoveTowards(lightCurrent, lightTarget, Time.deltaTime);

        //if (Input.GetKeyDown(KeyCode.Alpha2) && phase == GamePhase.Main)
        //    handler.link.Send(new MeetingRequested());
        if (Input.GetKeyDown(KeyCode.F5))
            handler.link.Send(new RestartRequested());

        if (Input.GetKeyDown(KeyCode.F4))
        {
            showAccessibilityToggle = !showAccessibilityToggle;
            accessibilityToggle.SetActive(showAccessibilityToggle);
        }

        for (int i = 0; i < 10; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + (i + 1) % 10))
                Sabotage(i);
        }

        targetMarker.SetActive(false);
        killButton.gameObject.SetActive(player.role == 1 && player.IsAlive);
        reportButton.gameObject.SetActive(player.IsAlive);
        itemButton.gameObject.SetActive(player.IsAlive);
        hud.gameObject.SetActive(phase == GamePhase.Main);
        taskbar.gameObject.SetActive(
            phase == GamePhase.Main || 
            phase == GamePhase.Discussion || 
            phase == GamePhase.Voting || 
            phase == GamePhase.EndOfMeeting);

        if (phase == GamePhase.Main && player.IsAlive)
        {
            // Kill
            if (player.role == 1 || knifeItem)
            {
                killTarget = ulong.MaxValue;
                float targetDistance = settings.killRange;
                if (player.killCooldown < time || knifeItem)
                {
                    foreach (var n in handler.mobs)
                    {
                        if(n.Value.role == 0 || knifeItem )
                        {

                            if (n.Value.IsAlive == true && n.Value.gameObject.activeSelf && !n.Value.inLocker && n.Key != handler.playerMobId)
                            {
                                float distance = Vector2.Distance(player.transform.position, n.Value.transform.position);
                                if (distance < targetDistance)
                                {
                                    killTarget = n.Key;
                                    targetDistance = distance;
                                }
                            }
                        }
                    }
                    killCooldownText.text = "";
                }
                if(player.killCooldown > time)
                {
                    killCooldownText.text = ((player.killCooldown - time + 999999999) / 1000000000).ToString();
                }
                if (!player.canMove)
                    killTarget = ulong.MaxValue;
                killButton.interactable = (killTarget != ulong.MaxValue && player.killCooldown < time);
                if (killTarget != ulong.MaxValue)
                {
                    targetMarker.SetActive(true);
                    targetMarker.transform.position = handler.mobs[killTarget].transform.position;
                    if (Input.GetKeyDown(KeyCode.Q) && !knifeItem)
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
                if (!player.canMove)
                    reportTarget = ulong.MaxValue;
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
                if (matchmaker.lobby.Length == 0)
                    text.text = "Requesting vacant lobby...";
                else
                    text.text = "Requesting lobby " + GetCensoredLobby() + "...";
                break;
            case ConnectionState.ConnectingToLobby:
                text.text = "Connecting to lobby " + GetCensoredLobby() + "...";
                break;
            case ConnectionState.JoiningLobby:
                text.text = "Joining lobby " + GetCensoredLobby() + "...";
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
                            text.text = "Setup " + GetCensoredLobby();
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
    }

    public void SetGamePhase(GamePhase phase, long timer, GamePhase previous)
    {
        popup.DeactivatePopup();

        if (phase == GamePhase.Setup || phase == GamePhase.GameOver)
        {
            taskManager.tasks.Clear();
            taskManager.sabotageTasks.Clear();
            for (int i = taskManager.indicators.Count - 1; i >= 0 ; i--)
                Destroy(taskManager.indicators[i]);
            taskManager.indicators.Clear();
            lightTarget = 1.0f;
            lightCurrent = 1.0f;
        }

        if (phase == GamePhase.Main && previous == GamePhase.Intro)
        {
            player.emergencyButtonsLeft = (int)settings.emergencyMeetingsPerPlayer;
            globalLight.intensity = 0.2f;
            areaText.text = "";
            areaText.rectTransform.anchoredPosition = new Vector2(0, -65);

            Dictionary<ulong, Mob> actualMobs = new Dictionary<ulong, Mob>();
            foreach (KeyValuePair<ulong, Mob> mob in handler.mobs)
                if (mob.Value.IsAlive)
                    actualMobs.Add(mob.Key, mob.Value);
            if (FindObjectOfType<MedicalMonitor>())
                FindObjectOfType<MedicalMonitor>().SetMobs(actualMobs);
            //GameObject.FindWithTag("Medical").GetComponent<MedicalMonitor>().mobs = actualMobs.Values.ToArray();
        }

        if (phase == GamePhase.Main)
        {
            roundTimer = 0f;
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

        //Sloppy prototype stuff
        if (phase == GamePhase.Intro && previous == GamePhase.Setup && !showAccessibilityToggle)
                accessibilityToggle.SetActive(false);

        if (phase == GamePhase.Setup)
            accessibilityToggle.SetActive(true);
    }

    public void Kill()
    {
        if (killTarget != ulong.MaxValue)
        {
            KillAttempted message = new KillAttempted
            {
                target = killTarget,
                time = time,
                knife = knifeItem
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
            handler.link.Send(new GameStartRequested());
        }
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

    public string GetCensoredLobby()
    {
        return streamerMode ? "*".PadRight(matchmaker.lobby.Length, '*') : matchmaker.lobby;
    }

    public void CreateInfoPopup(string info)
    {
        //if (!info.Contains("index")) Destroy(GameObject.FindWithTag("Info popup")); // what was this for??
        var go = Instantiate(infoPopupPrefab, canvas.transform);
        go.GetComponentInChildren<Text>().text = info;
    }

    public Button CreateConfirmPopup(string text)
    {
        if (currentConfirmPopup != null)
            Destroy(currentConfirmPopup);
        currentConfirmPopup = Instantiate(confirmPopupPrefab, canvas.transform);
        currentConfirmPopup.GetComponentInChildren<Text>().text = text;
        return currentConfirmPopup.GetComponentInChildren<Button>();
    }

    public void UpdateHidden()
    {
        foreach (KeyValuePair<ulong, Mob> entry in handler.mobs)
        {
            if (player.inCamo && !entry.Value.inLocker && !player.inLocker)
            {
                entry.Value.Reveal();
            }
            if ((!player.inCamo && entry.Value.inCamo) || (player.inLocker && entry.Value.inCamo))
            {
                entry.Value.Hide();
            }
        }
    }

    public void SpawnSmokeGrenade(Vector2 pos)
    {
        GameObject sg = Instantiate(smokeGrenadePrefab, pos, Quaternion.identity);
    }

    public void ChangeColor(Vector3 color)
    {
        player.SetColor(color.x, color.y, color.z);
        handler.link.Send(new SetMobColor { color = color });
        Debug.Log("affirmative");
    }
}
