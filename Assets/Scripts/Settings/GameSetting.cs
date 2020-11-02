using System.Reflection;
using UnityEngine.UI;

public abstract class GameSetting
{
    protected string name;
    protected FieldInfo field;
    protected GameController game;

    public GameSetting(string name, string field, GameController game)
    {
        this.name = name;
        this.field = typeof(GameSettings).GetField(field);
        this.game = game;
    }

    public abstract void CreateInput(GameSettingsManager manager);

    public abstract void UpdateInputDisplay();

    public abstract Selectable GetSelectable();
}
