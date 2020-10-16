

public class GameSettingInteger : GameSetting
{
    public override string Get()
    {
        return value.ToString();
    }

    public override void Set(string text)
    {
        value = long.Parse(text);
    }
}
