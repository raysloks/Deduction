

public class GameSettingTime : GameSetting
{
    public override string Get()
    {
        return (value / 1000000000).ToString() + "s";
    }

    public override void Set(string text)
    {
        value = long.Parse(text.Replace("s", "")) * 1000000000;
    }
}
