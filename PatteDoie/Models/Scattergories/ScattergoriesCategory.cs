namespace PatteDoie.Models.Scattergories
{
    public class ScattergoriesCategory
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public List<ScattergoriesGame> Games { get; set; } = [];
    }
}
