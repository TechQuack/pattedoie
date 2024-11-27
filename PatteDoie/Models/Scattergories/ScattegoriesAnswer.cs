namespace PatteDoie.Models.Scattergories
{
    public class ScattegoriesAnswer
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public required ScattergoriesCategory Category { get; set; }
    }
}
