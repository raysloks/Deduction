

public class GameSettingReal : GameSetting
{
    public long resolution;

    public override string Get()
    {
        return ((float)value / resolution).ToString();
    }

    public override void Set(string text)
    {
        value = (long)(float.Parse(text) * resolution);
    }
}
