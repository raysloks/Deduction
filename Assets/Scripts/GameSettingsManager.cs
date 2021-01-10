using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class GameSettingsManager : MonoBehaviour
{
    public List<GameSetting> settings;

    public RectTransform window;
    public RectTransform content;
    public GameObject textFieldPrefab;
    public GameObject togglePrefab;
    public GameObject dropdownPrefab;

    public GameController game;

    [Header("High Contrast")]
    public GameObject grid1;
    public GameObject grid2;
    public GameObject colgrid1;
    public GameObject colgrid2;
    public GameObject spriteCollision;
    public GameObject minigameInitiator;
    public GameObject camoHolder;
    public GameObject ItemContainers;
    public GameObject lockerHolder;
    private bool changed = false;
    private SpriteRenderer[] sr;

    public Color collisionColor;
    public Color interactableColor;
    public Color backgroundColor;
    public Color camoColor;

    public bool accessibilityMode;

    private void Awake()
    {
        settings = new List<GameSetting>
        {
            new GameSettingDropdown("Map", "map", game, new List<string>{ "Hangar", "Base"/*, "[Prototyp]"*/ }),
            new GameSettingInputField<long>("Number Of Spies", "impostorCount", game),
            //new GameSettingInputField<long>("Votes Per Player", "votesPerPlayer", game),
            new GameSettingInputField<long>("Emergency Meetings Per Player", "emergencyMeetingsPerPlayer", game),
            new GameSettingTime("Emergency Meeting Cooldown", "emergencyMeetingCooldown", game),
            new GameSettingTime("Discussion Time", "discussionTime", game),
            new GameSettingTime("Vote Time", "voteTime", game),
            new GameSettingInputField<float>("Player Speed", "playerSpeed", game),
            new GameSettingInputField<float>("Soldier Vision", "crewmateVision", game),
            new GameSettingInputField<float>("Spy Vision", "impostorVision", game),
            new GameSettingTime("Kill Cooldown", "killCooldown", game),
            new GameSettingInputField<float>("Kill Range", "killRange", game),
            new GameSettingTime("Sabotage Cooldown", "sabotageCooldown", game),
            //new GameSettingToggle("Kill Victory Enabled", "killVictoryEnabled", game),
            //new GameSettingToggle("Kill On Ties", "killOnTies", game),
            //new GameSettingToggle("Enable Skip Button", "enableSkipButton", game),
            new GameSettingInputField<long>("Short Tasks", "shortTaskCount", game),
            new GameSettingInputField<long>("Long Tasks", "longTaskCount", game),
            new GameSettingDropdown("Taskbar Update Style", "taskbarUpdateStyle", game, new List<string>{ "Instant", "End of Meeting", "Start of Meeting" }),
            new GameSettingToggle("Hide Votes Until Everyone Has Voted", "showVotesWhenEveryoneHasVoted", game),
            new GameSettingToggle("Anonymous Votes", "anonymousVotes", game),
            new GameSettingToggle("[DEV]Game Over Enabled", "gameOverEnabled", game)
        };

        game.settings.crewmateVision = 5.0f;
        game.settings.playerSpeed = 5.0f;

        foreach (var setting in settings)
            setting.CreateInput(this);

        for (int i = 0; i < settings.Count; ++i)
        {
            settings[i].GetSelectable().navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = settings[Utility.Mod(i - 1, settings.Count)].GetSelectable(),
                selectOnDown = settings[Utility.Mod(i + 1, settings.Count)].GetSelectable()
            };
        }
    }

    private void Start()
    {
        if(spriteCollision != null)
        {
            sr = spriteCollision.GetComponentsInChildren<SpriteRenderer>();
        }
    }

    private void Update()
    {
        //window.gameObject.SetActive(game.phase == GamePhase.Setup && game.timer == 0);
        if (Input.GetKeyDown(KeyCode.F6))
        {
            if(changed == false)
            {
                changed = true;
                colorChangeCollision(collisionColor);
                colorChangeInteractables(interactableColor);
                colorChangeBackground(backgroundColor);
                colorChangeCamo(camoColor);
            }
            else
            {
                changed = false;

                colorChangeCollision(Color.white);
                colorChangeInteractables(Color.white);
                colorChangeBackground(Color.white);
                colorChangeCamo(Color.white);
            }

        }
    }

    public void colorChangeCamo(Color c)
    {
        foreach (Transform s in camoHolder.transform)
        {           
             s.GetComponent<SpriteRenderer>().color = c;          
        }
    }

    public void colorChangeBackground(Color c)
    {

        grid1.GetComponent<Tilemap>().color = Color.gray;

        grid2.GetComponent<Tilemap>().color = Color.gray;
    }

    public void colorChangeCollision(Color c)
    {
        if (spriteCollision != null)
        {
            foreach (SpriteRenderer s in sr)
            {
                s.color = c;
            }
        }

        colgrid1.GetComponent<Tilemap>().color = c;
        colgrid2.GetComponent<Tilemap>().color = c;
    }

    public void colorChangeInteractables(Color c)
    {
        if (minigameInitiator != null)
        {
            foreach (Transform s in minigameInitiator.transform)
            {
                if (s.gameObject.name != "outline")
                {
                    s.GetComponent<SpriteRenderer>().color = c;
                }

            }
        }

        if (ItemContainers != null)
        {
            foreach (Transform s in ItemContainers.transform)
            {
                if (s.gameObject.name != "outline")
                {
                    s.GetComponent<SpriteRenderer>().color = c;
                }

            }
        }

        if (lockerHolder != null)
        {
            foreach (Transform s in lockerHolder.transform)
            {
                if (s.gameObject.name != "outline")
                {
                    s.GetComponent<SpriteRenderer>().color = c;
                }

            }
        }
    }

    public void UpdateInputDisplay()
    {
        foreach (var setting in settings)
            setting.UpdateInputDisplay();
    }

    public void SendSettings()
    {
        game.handler.link.Send(game.settings);
    }

    public void SetInteractable(bool interactable)
    {
        foreach (var setting in settings)
            setting.GetSelectable().interactable = interactable;
    }

    public void SetAccessibility(bool value)
    {
        accessibilityMode = value;
    }
}
