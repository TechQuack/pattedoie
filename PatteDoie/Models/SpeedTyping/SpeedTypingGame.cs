namespace PatteDoie.Models.SpeedTyping
{
    public class SpeedTypingGame
    {
        public Guid Id { get; set; }
        public ICollection<SpeedTypingScore> Scores { get; set; }
        public ICollection<SpeedTypingTimeProgress> TimeProgresses { get; set; }
        public DateTime LaunchTime { get; set; }
        public string[] Words { get; set; }
    }
}
