using PatteDoie.Models.Platform;

namespace PatteDoie.Models.SpeedTyping
{
    public class SpeedTypingPlayer
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public required User User { get; set; }
    }
}
