using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;

namespace PatteDoie.Rows.Scattegories
{
    public class ScattergoriesPlayerRow
    {

        public Guid Id { get; set; }
        public int Score { get; set; }
        public required User User { get; set; }
        public required List<ScattegoriesAnswer> Answers { get; set; }
        public bool IsHost { get; set; }

    }
}
