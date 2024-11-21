namespace PatteDoie.Models.SpeedTyping
{
    public class SpeedTypingGame
    {
        public Guid Id { get; set; }
        public List<SpeedTypingPlayer> Players { get; set; } = [];
        public List<SpeedTypingTimeProgress> TimeProgresses { get; set; } = [];
        public DateTime LaunchTime { get; set; }
        public List<string> Words { get; set; } = [];
    }
}
