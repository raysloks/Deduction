using UnityEngine.UI;

public class GameSettingDropdown : GameSetting
{
    private Dropdown dropdown;

    public GameSettingDropdown(string name, string field, GameController game) : base(name, field, game)
    {
    }

    public override void CreateInput(GameSettingsManager manager)
    {
        var go = UnityEngine.Object.Instantiate(manager.dropdownPrefab, manager.content);
        go.GetComponentInChildren<Text>().text = name;
        dropdown = go.GetComponentInChildren<Dropdown>();
        dropdown.value = (int)field.GetValue(game.settings);
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
        dropdown.SetValueWithoutNotify((int)field.GetValue(game.settings));
    }
}