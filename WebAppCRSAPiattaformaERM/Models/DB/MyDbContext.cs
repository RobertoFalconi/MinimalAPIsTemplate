namespace MinimalSPAwithAPIs.Models.DB;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MyFirstApiDb> MyFirstApiDb { get; set; }

    public virtual DbSet<MyUsersDb> MyUsers { get; set; }

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

            var connstring = config.GetConnectionString("MinimalSPAwithAPIs")!;

            if (!string.IsNullOrEmpty(connstring)) optionsBuilder.UseSqlServer(connstring);
            else optionsBuilder.UseSqlServer("connstring");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MyUsersDb>(entity =>
        {
            entity.HasKey(e => e.PrimaryKey).HasName("PrimaryKey");

            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.FiscalCode).HasMaxLength(16);
            entity.Property(e => e.LastUpdateApp).HasMaxLength(20);
            entity.Property(e => e.LastUpdateUser).HasMaxLength(16);
            entity.Property(e => e.Surname).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.State)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.EmployeeID).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Profile).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(50);

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
