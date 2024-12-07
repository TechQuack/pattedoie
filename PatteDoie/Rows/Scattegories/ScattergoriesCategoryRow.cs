using PatteDoie.Models.Scattergories;

namespace PatteDoie.Rows.Scattegories
{
    public class ScattergoriesCategoryRow
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public List<ScattergoriesGame> Games { get; set; } = [];
    }
}
