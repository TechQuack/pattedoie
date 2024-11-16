namespace PatteDoie.Models.Platform
{
    public class PlatformGame
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public PlatformHighScore[] HighScores { get; set; }

        public int Min_players { get; set; }

        public int Max_players { get; set; }

    }
}
