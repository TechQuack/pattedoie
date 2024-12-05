using PatteDoie.Rows.Platform;

namespace PatteDoie.Rows.SpeedTyping
{
    public class SpeedTypingPlayerRow
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public required PlatformUserRow User { get; set; }
        public int SecondsToFinish { get; set; }
    }
}
