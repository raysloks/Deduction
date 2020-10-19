

public class GameSettingBoolean : GameSetting
{
    public override string Get()
    {

        return value.ToString();
    }
    
    public bool GetBool()
    {
        if (value == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }

    public override void Set(string text)
    {
        value = long.Parse(text);
    }
}