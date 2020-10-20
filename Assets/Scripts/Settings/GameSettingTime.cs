public class GameSettingTime : GameSettingInputField<long>
{
    public GameSettingTime(string name, string field, GameController game) : base(name, field, game)
    {
    }

    protected override string Get()
    {
        return ((long)field.GetValue(game.settings) / 1000000000).ToString() + "s";
    }

    protected override void Set(string text)
    {
        field.SetValue(game.settings, long.Parse(text.Replace("s", "")) * 1000000000);
    }
}
