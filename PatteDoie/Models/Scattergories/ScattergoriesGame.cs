namespace PatteDoie.Models.Scattergories
{
    public class ScattergoriesGame
    {
        public Guid Id { get; set; }
        public ScattergoriesPlayer[] Players { get; set; }
        public int MaxRound { get; set; }
        public int CurrentRound { get; set; }
        public char CurrentLetter { get; set; }
        public ScattergoriesCategory[] Categories { get; set; }
    }
}
