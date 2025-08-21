using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Company> Company { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Buildings> Buildings { get; set; }
        public DbSet<Floors> Floors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<Rooms>()
                    .HasKey(r => new { r.RoomId, r.CompanyId, r.BuildingId, r.FloorId });

        }
}