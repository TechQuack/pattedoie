using PatteDoie.Models.Platform;

namespace PatteDoie.Rows.Platform
{
    public class PlatformLobbyRow
    {

        public Guid Id { get; set; }

        public List<User> Users { get; set; } = [];

        public required User Creator { get; set; }

        public GameRow? Game { get; set; }

        public string? Password { get; set; }

        public bool Started { get; set; }

    }
}
