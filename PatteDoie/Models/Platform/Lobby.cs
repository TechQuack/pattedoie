namespace PatteDoie.Models.Platform
{
    public class Lobby
    {

        public Guid Id { get; set; }

        public List<User> Users { get; set; } = [];

        public required User Creator { get; set; }

        public Guid GameId { get; set; }

        public string? Password { get; set; }

        public bool Started { get; set; } = false;

    }
}
