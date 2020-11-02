using UnityEngine.UI;

public class GameSettingToggle : GameSetting
{
    private Toggle toggle;

    public GameSettingToggle(string name, string field, GameController game) : base(name, field, game)
    {
    }

    public override void CreateInput(GameSettingsManager manager)
    {
        var go = UnityEngine.Object.Instantiate(manager.togglePrefab, manager.content);
        go.GetComponentInChildren<Text>().text = name;
        toggle = go.GetComponentInChildren<Toggle>();
        toggle.isOn = (bool)field.GetValue(game.settings);
        toggle.onValueChanged.AddListener((bool value) =>
        {
            object settings = game.settings;
            field.SetValue(settings, value);
            game.settings = (GameSettings)settings;
            manager.SendSettings();
        });
    }

    public override void UpdateInputDisplay()
    {
        toggle.SetIsOnWithoutNotify((bool)field.GetValue(game.settings));
    }

    public override Selectable GetSelectable()
    {
        return toggle;
    }
}