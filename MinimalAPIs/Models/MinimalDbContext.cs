using Microsoft.EntityFrameworkCore;

namespace MinimalAPIs.Models
{
    public partial class MinimalDbContext : DbContext
    {
        public MinimalDbContext()
        {
        }

        public MinimalDbContext(DbContextOptions<MinimalDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("No connection string set for database.");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
