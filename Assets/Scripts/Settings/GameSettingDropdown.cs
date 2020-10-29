using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameSettingDropdown : GameSetting
{
    private List<string> values;
    private Dropdown dropdown;

    public GameSettingDropdown(string name, string field, GameController game, List<string> values) : base(name, field, game)
    {
        this.values = values;
    }

    public override void CreateInput(GameSettingsManager manager)
    {
        var go = UnityEngine.Object.Instantiate(manager.dropdownPrefab, manager.content);
        go.GetComponentInChildren<Text>().text = name;
        dropdown = go.GetComponentInChildren<Dropdown>();
        dropdown.AddOptions(values);
        dropdown.value = (int)Convert.ChangeType(field.GetValue(game.settings), typeof(int));
        dropdown.onValueChanged.AddListener((int value) =>
        {
            object settings = game.settings;
            field.SetValue(settings, value);
            game.settings = (GameSettings)settings;
            manager.SendSettings();
        });
    }

    public override void UpdateInputDisplay()
    {
        dropdown.SetValueWithoutNotify((int)Convert.ChangeType(field.GetValue(game.settings), typeof(int)));
    }
}