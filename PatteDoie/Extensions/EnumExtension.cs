using PatteDoie.Attributes;

namespace PatteDoie.Extensions;

public static class EnumExtension
{
    public static string GetDescription(this Enum value)
    {
        var enumType = value.GetType();
        var name = Enum.GetName(enumType, value);
        return enumType.GetField(name ?? "")?.GetCustomAttributes(false).OfType<DescriptionAttribute>().SingleOrDefault()?.Description ?? "";
    }
}
