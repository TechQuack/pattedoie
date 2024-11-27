namespace PatteDoie.Models.SpeedTyping
{
    public class SpeedTypingTimeProgress
    {
        public Guid Id { get; set; }
        public SpeedTypingPlayer Player { get; set; }
        public DateTime TimeProgress { get; set; }
    }
}
