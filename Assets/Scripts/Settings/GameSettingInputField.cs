using System;
using UnityEngine.UI;

public class GameSettingInputField<T> : GameSetting where T : IEquatable<T>
{
    private InputField inputField;

    public GameSettingInputField(string name, string field, GameController game) : base(name, field, game)
    {
    }

    public override void CreateInput(GameSettingsManager manager)
    {
        var go = UnityEngine.Object.Instantiate(manager.textFieldPrefab, manager.content);
        go.GetComponentInChildren<Text>().text = name;
        inputField = go.GetComponentInChildren<InputField>();
        inputField.text = Get();
        inputField.onEndEdit.AddListener((string text) =>
        {
            //T prev = (T)field.GetValue(game.settings);
            Set(text);
            //T value = (T)field.GetValue(game.settings);
            //if (prev.Equals(value))
            //    inputField.SetTextWithoutNotify(Get());
            //else
            manager.SendSettings();
        });
    }

    public override void UpdateInputDisplay()
    {
        inputField.SetTextWithoutNotify(Get());
    }

    public override Selectable GetSelectable()
    {
        return inputField;
    }

    protected virtual string Get()
    {
        return field.GetValue(game.settings).ToString();
    }

    protected virtual void Set(string text)
    {
        object settings = game.settings;
        field.SetValue(settings, Convert.ChangeType(text, typeof(T)));
        game.settings = (GameSettings)settings;
    }
}
