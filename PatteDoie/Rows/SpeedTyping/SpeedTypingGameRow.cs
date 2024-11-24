using PatteDoie.Models.SpeedTyping;

namespace PatteDoie.Rows.SpeedTypingGame
{
    public class SpeedTypingGameRow
    {
        public Guid Id { get; set; }
        public SpeedTypingPlayer[] Scores { get; set; }
        public SpeedTypingTimeProgress[] TimeProgresses { get; set; }
        public DateTime LaunchTime { get; set; }
        public string[] Words { get; set; }
    }
}
