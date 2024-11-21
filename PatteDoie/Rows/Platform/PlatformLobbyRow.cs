using PatteDoie.Models.Platform;

namespace PatteDoie.Rows.Platform
{
    public class PlatformLobbyRow
    {

        public Guid Id { get; set; }

        public List<PlatformUser> Users { get; set; } = [];

        public required PlatformUser Creator { get; set; }

        public Guid GameId { get; set; }

        public string? Password { get; set; }

        public bool Started { get; set; }

    }
}
