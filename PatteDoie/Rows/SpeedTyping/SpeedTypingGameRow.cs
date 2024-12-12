using PatteDoie.Rows.Platform;
using PatteDoie.Rows.SpeedTyping;

namespace PatteDoie.Rows.SpeedTypingGame
{
    public class SpeedTypingGameRow
    {
        public Guid Id { get; set; }
        public List<SpeedTypingPlayerRow> Scores { get; set; } = [];
        public List<SpeedTypingTimeProgressRow> TimeProgresses { get; set; } = [];
        public DateTime LaunchTime { get; set; }
        public string[] Words { get; set; }
        public required PlatformLobbyRow Lobby { get; set; }
    }
}
