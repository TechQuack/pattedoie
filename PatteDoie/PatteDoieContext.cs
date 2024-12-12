using Microsoft.EntityFrameworkCore;
using PatteDoie.Enums;
using PatteDoie.Extensions;
using PatteDoie.Models.Platform;
using PatteDoie.Models.Scattergories;

namespace PatteDoie;

public partial class PatteDoieContext : DbContext
{
    public PatteDoieContext()
    {
    }

    public PatteDoieContext(DbContextOptions<PatteDoieContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<Game>().HasData(
            new Game { Id = Guid.NewGuid(), Name = GameType.Scattergories.GetDescription(), MinPlayers = 2, MaxPlayers = 8 },
            new Game { Id = Guid.NewGuid(), Name = GameType.SpeedTyping.GetDescription(), MinPlayers = 1, MaxPlayers = 5 }
        );

        modelBuilder.Entity<ScattergoriesCategory>().HasData(
            new ScattergoriesCategory { Id = Guid.NewGuid(), Name = "country" },
            new ScattergoriesCategory { Id = Guid.NewGuid(), Name = "animal" },
            new ScattergoriesCategory { Id = Guid.NewGuid(), Name = "firstname" },
            new ScattergoriesCategory { Id = Guid.NewGuid(), Name = "clothes" },
            new ScattergoriesCategory { Id = Guid.NewGuid(), Name = "sport" },
            new ScattergoriesCategory { Id = Guid.NewGuid(), Name = "occupation" },
            new ScattergoriesCategory { Id = Guid.NewGuid(), Name = "fruit or vegetable" },
            new ScattergoriesCategory { Id = Guid.NewGuid(), Name = "brand" },
            new ScattergoriesCategory { Id = Guid.NewGuid(), Name = "famous person" },
            new ScattergoriesCategory { Id = Guid.NewGuid(), Name = "game" },
            new ScattergoriesCategory { Id = Guid.NewGuid(), Name = "movie/tv show" }
        );
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<PatteDoie.Models.SpeedTyping.SpeedTypingTimeProgress> SpeedTypingTimeProgress { get; set; } = default!;

    public DbSet<PatteDoie.Models.SpeedTyping.SpeedTypingPlayer> SpeedTypingPlayer { get; set; } = default!;

    public DbSet<PatteDoie.Models.SpeedTyping.SpeedTypingGame> SpeedTypingGame { get; set; } = default!;

    public DbSet<PatteDoie.Models.Platform.User> PlatformUser { get; set; } = default!;

    public DbSet<PatteDoie.Models.Scattergories.ScattergoriesCategory> ScattergoriesCategory { get; set; } = default!;

    public DbSet<PatteDoie.Models.Platform.Lobby> PlatformLobby { get; set; } = default!;

    public DbSet<PatteDoie.Models.Platform.HighScore> PlatformHighScore { get; set; } = default!;

    public DbSet<PatteDoie.Models.Scattergories.ScattergoriesPlayer> ScattergoriesPlayer { get; set; } = default!;

    public DbSet<PatteDoie.Models.Platform.Game> PlatformGame { get; set; } = default!;

    public DbSet<PatteDoie.Models.Scattergories.ScattergoriesGame> ScattergoriesGame { get; set; } = default!;

    public DbSet<PatteDoie.Models.Scattergories.ScattergoriesAnswer> ScattergoriesAnswer { get; set; } = default!;
}
