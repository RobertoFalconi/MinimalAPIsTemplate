namespace MVCwithMediatRandCQRS.DbModels;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Utenti> Utenti { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables();

            IConfiguration config = builder.Build();

            var connstring = config.GetConnectionString("MVCwithMediatRandCQRS.DB.Main")!;

            if (!string.IsNullOrEmpty(connstring)) optionsBuilder.UseSqlServer(connstring);
            else optionsBuilder.UseSqlServer("connstring");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Utenti>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Id");
            entity.Property(e => e.Nome).HasMaxLength(20);
            entity.Property(e => e.Cognome).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(20);

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
