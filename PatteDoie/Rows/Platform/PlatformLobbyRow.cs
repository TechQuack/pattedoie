using PatteDoie.Models.Platform;

namespace PatteDoie.Rows.Platform
{
    public class PlatformLobbyRow
    {

        public Guid Id { get; set; }

        public User[] Users { get; set; } = [];

        public required User Creator { get; set; }

        public Guid GameId { get; set; }

        public string? Password { get; set; }

        public bool Started { get; set; }

    }
}
