﻿using Microsoft.EntityFrameworkCore;

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
}
