using PatteDoie.Models.Platform;

namespace PatteDoie.Rows.Platform
{
    public class PlatformLobbyRow
    {

        public Guid Id { get; set; }

        public PlatformUser[] users { get; set; }

        public PlatformUser creator { get; set; }

        public Guid gameId { get; set; }

        public string password { get; set; }

        public bool started { get; set; }

    }
}
