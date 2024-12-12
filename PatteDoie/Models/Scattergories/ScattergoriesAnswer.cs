namespace PatteDoie.Models.Scattergories
{
    public class ScattergoriesAnswer
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public required ScattergoriesCategory Category { get; set; }
        public required bool IsChecked { get; set; }
    }
}
