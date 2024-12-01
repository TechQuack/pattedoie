namespace PatteDoie.Models.Scattergories
{
    public class ScattergoriesGame : BaseGame
    {
        public Guid Id { get; set; }
        public List<ScattergoriesPlayer> Players { get; set; } = [];
        public int MaxRound { get; set; }
        public int CurrentRound { get; set; }
        public char CurrentLetter { get; set; }
        public bool IsHostCheckingPhase { get; set; }
        public List<ScattergoriesCategory> Categories { get; set; } = [];
    }
}
