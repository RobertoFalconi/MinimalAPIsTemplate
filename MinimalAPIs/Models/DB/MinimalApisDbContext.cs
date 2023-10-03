﻿namespace MinimalAPIs.Models.DB;

public partial class MinimalApisDbContext : DbContext
{
    public MinimalApisDbContext()
    {
    }

    public MinimalApisDbContext(DbContextOptions<MinimalApisDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Nlog> Nlog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Nlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NLog__3214EC07BD49837C");

            entity.ToTable("NLog");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Logged).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
