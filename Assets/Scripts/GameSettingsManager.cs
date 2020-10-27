using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameSettingsManager : MonoBehaviour
{
    public List<GameSetting> settings;

    public RectTransform window;
    public RectTransform content;
    public GameObject textFieldPrefab;
    public GameObject togglePrefab;
    public GameObject dropdownPrefab;

    public GameController game;

    private void Awake()
    {
        settings = new List<GameSetting>
        {
            new GameSettingInputField<long>("Impostor Count", "impostorCount", game),
            new GameSettingInputField<long>("Votes Per Player", "votesPerPlayer", game),
            new GameSettingInputField<long>("Emergency Meetings Per Player", "emergencyMeetingsPerPlayer", game),
            new GameSettingTime("Emergency Meeting Cooldown", "emergencyMeetingCooldown", game),
            new GameSettingTime("Kill Cooldown", "killCooldown", game),
            new GameSettingInputField<float>("Kill Range", "killRange", game),
            new GameSettingTime("Vote Time", "voteTime", game),
            new GameSettingTime("Discussion Time", "discussionTime", game),
            new GameSettingToggle("Kill Victory Enabled", "killVictoryEnabled", game),
            new GameSettingInputField<float>("Crewmate Vision", "crewmateVision", game),
            new GameSettingInputField<float>("Impostor Vision", "impostorVision", game),
            new GameSettingInputField<float>("Player Speed", "playerSpeed", game),
            new GameSettingToggle("Kill On Ties", "killOnTies", game),
            new GameSettingToggle("Enable Skip Button", "enableSkipButton", game),
            new GameSettingToggle("Hide Votes Until Everyone Has Voted", "showVotesWhenEveryoneHasVoted", game),
            new GameSettingToggle("Anonymous Votes", "anonymousVotes", game),
        };

        game.settings.crewmateVision = 5.0f;
        game.settings.playerSpeed = 4.0f;

        foreach (var setting in settings)
            setting.CreateInput(this);
    }

    private void Update()
    {
        window.gameObject.SetActive(game.phase == GamePhase.Setup && game.timer == 0);
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
}
