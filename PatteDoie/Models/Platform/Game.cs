namespace PatteDoie.Models.Platform
{
    public class Game
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;    

        public List<HighScore> HighScores { get; set; } = [];

        public int MinPlayers { get; set; }

        public int MaxPlayers { get; set; }

        public List<Lobby> Lobbys { get; set; } = [];

    }
}
