namespace PatteDoie.Rows.SpeedTyping
{
    public class SpeedTypingTimeProgressRow
    {
        public Guid Id { get; set; }
        public SpeedTypingPlayerRow Player { get; set; }
        public DateTime TimeProgress { get; set; }
    }
}
