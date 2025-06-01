using Microsoft.EntityFrameworkCore;

namespace MyAspNetCoreProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Location> Locations { get; set; }
        public DbSet<PythonResponse> PythonResponses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LocationImage> LocationImages { get; set; }  // Yeni eklenen DbSet

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        public DbSet<LocationAnalysis> LocationAnalyses { get; set; } // ✅ Bunu ekle


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User ve Location: Birden çoğa ilişki
            modelBuilder.Entity<Location>()
                .HasOne(l => l.User)
                .WithMany(u => u.Locations)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Location ve LocationImage: Birden çoğa ilişki
            modelBuilder.Entity<Location>()
                .HasMany(l => l.Images)
                .WithOne(li => li.Location)
                .HasForeignKey(li => li.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Location ve WeatherForecast: Birden çoğa ilişki
            modelBuilder.Entity<WeatherForecast>()
                .HasOne(wf => wf.Location)
                .WithMany(l => l.WeatherForecasts)
                .HasForeignKey(wf => wf.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
        .HasMany(l => l.Analyses)
        .WithOne(a => a.Location)
        .HasForeignKey(a => a.LocationId)
        .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
