



using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Used_Car_Shop.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Cars> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<OrderToSell> OrdersToSell { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model>()
            .HasOne(m => m.Brand)
            .WithMany(b => b.models)
            .HasForeignKey(m => m.brand_id)
            .OnDelete(DeleteBehavior.NoAction);
            var brandjson = File.ReadAllText(FileSettings.BrandsFilePath);
            var modeljson =File.ReadAllText(FileSettings.ModelsFilePath);

            var brands = JsonSerializer.Deserialize<List<Brand>>(brandjson);
            var models = JsonSerializer.Deserialize<List<Model>>(modeljson);
            modelBuilder.Entity<Brand>().HasData(brands);
            modelBuilder.Entity<Model>().HasData(models);
            modelBuilder.Entity<OrderToSell>()
            .HasOne(p => p.user)
            .WithMany()
            .HasForeignKey(p => p.userId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole()
            {
                 Id = Guid.NewGuid().ToString(),
                 Name = "Admin",
                 NormalizedName = "ADMIN"
            },
            new IdentityRole()
            {
                 Id = Guid.NewGuid().ToString(),
                 Name = "User",
                 NormalizedName = "USER"
            });
            base.OnModelCreating(modelBuilder);

        }

    }
}
