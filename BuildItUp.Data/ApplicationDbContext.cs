using Microsoft.EntityFrameworkCore;
using BuildItUp.Models.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BuildItUp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Company → Engine
            modelBuilder.Entity<Engine>()
                .HasOne(e => e.Company)
                .WithMany(c => c.Engines)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Car → Engine
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Engine)
                .WithMany(e => e.Cars)
                .HasForeignKey(c => c.EngineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Car → Company 
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Company)
                .WithMany(cmp => cmp.Cars)
                .HasForeignKey(c => c.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
