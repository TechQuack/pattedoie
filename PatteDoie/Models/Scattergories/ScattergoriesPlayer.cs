using PatteDoie.Models.Platform;

namespace PatteDoie.Models.Scattergories
{
    public class ScattergoriesPlayer
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public required User User { get; set; }
        public required List<ScattergoriesAnswer> Answers { get; set; }
        public bool IsHost { get; set; } 
    }
}
