﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.SpeedTyping;

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

public DbSet<PatteDoie.Models.SpeedTyping.SpeedTypingWord> SpeedTypingWord { get; set; } = default!;

public DbSet<PatteDoie.Models.SpeedTyping.SpeedTypingTimeProgress> SpeedTypingTimeProgress { get; set; } = default!;

public DbSet<PatteDoie.Models.SpeedTyping.SpeedTypingScore> SpeedTypingScore { get; set; } = default!;

public DbSet<PatteDoie.Models.SpeedTyping.SpeedTypingGame> SpeedTypingGame { get; set; } = default!;
}
