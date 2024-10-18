using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
}
