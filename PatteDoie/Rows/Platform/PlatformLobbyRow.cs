namespace PatteDoie.Rows.Platform
{
    public class PlatformLobbyRow
    {

        public Guid Id { get; set; }

        public List<PlatformUserRow> Users { get; set; } = [];

        public required PlatformUserRow Creator { get; set; }

        public required GameRow Game { get; set; }

        public string? Password { get; set; }

        public bool Started { get; set; }

    }
}
