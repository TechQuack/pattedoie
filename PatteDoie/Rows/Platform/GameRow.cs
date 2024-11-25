namespace PatteDoie.Rows.Platform;

public class GameRow
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int MinPlayers { get; set; }

    public int MaxPlayers { get; set; }
}
