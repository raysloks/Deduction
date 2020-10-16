
public abstract class GameSetting
{
    public string name;
    public long value;

    public abstract string Get();
    public abstract void Set(string text);
}
