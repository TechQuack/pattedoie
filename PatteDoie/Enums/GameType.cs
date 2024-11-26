using PatteDoie.Attributes;

namespace PatteDoie.Enums;

public enum GameType
{
    [Description("SpeedTyping")]
    SpeedTyping,
    [Description("Scattergories")]
    Scattergories
}

static class GameTypeHelper
{
    public static GameType GetGameTypeFromString(string value)
    {
        return value switch
        {
            "SpeedTyping" => GameType.SpeedTyping,
            "Scattergories" => GameType.Scattergories,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}