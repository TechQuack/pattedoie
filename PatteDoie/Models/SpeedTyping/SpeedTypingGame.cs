namespace PatteDoie.Models.SpeedTyping
{
    public class SpeedTypingGame
    {
        public Guid Id { get; set; }
        public SpeedTypingScore[] Scores { get; set; }
        public SpeedTypingTimeProgress[] TimeProgresses { get; set; }
        public DateTime LaunchTime { get; set; }
    }
}
