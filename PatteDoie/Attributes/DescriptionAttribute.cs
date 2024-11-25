namespace PatteDoie.Attributes;

public class DescriptionAttribute(string description) : Attribute
{
    public string Description { get; set; } = description;
}
