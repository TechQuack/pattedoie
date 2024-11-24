namespace PatteDoie.Models.Platform
{
    public class HighScore
    {
        public Guid Id { get; set; }

        public string PlayerName { get; set; } = string.Empty;

        public int Score { get; set; }

    }
}
