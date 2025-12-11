using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FlowerShop.Models;
using FlowerShop.Models;

namespace FlowerShop.Data
{
    public class FlowerShopContext : IdentityDbContext<Client, IdentityRole, string>
    {
        public FlowerShopContext(DbContextOptions<FlowerShopContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure all decimal properties to have precision of decimal(18, 2)
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                    {
                        property.SetColumnType("decimal(18, 2)");
                    }
                }
            }

            // Seed initial product data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Classic Rose Bouquet", Price = 68m, ImageUrl = "https://images.pexels.com/photos/7666520/pexels-photo-7666520.jpeg", Category = "Wedding", Color = "Red" },
                new Product { Id = 2, Name = "Peony Petal Ensemble", Price = 74m, ImageUrl = "https://images.pexels.com/photos/4993265/pexels-photo-4993265.jpeg", Category = "Wedding", Color = "Pink" },
                new Product { Id = 3, Name = "Spring Tulip Medley", Price = 59m, ImageUrl = "https://images.pexels.com/photos/1108650/pexels-photo-1108650.jpeg", Category = "Birthday", Color = "Mixed" },
                new Product { Id = 4, Name = "Garden Soirée Bouquet", Price = 62m, ImageUrl = "https://images.pexels.com/photos/70330/pexels-photo-70330.jpeg", Category = "Birthday", Color = "Mixed" },
                new Product { Id = 5, Name = "Velvet Rose Arrangement", Price = 72m, ImageUrl = "https://images.pexels.com/photos/34701055/pexels-photo-34701055.jpeg", Category = "Anniversary", Color = "Red" },
                new Product { Id = 6, Name = "Ranunculus Romance", Price = 80m, ImageUrl = "https://images.pexels.com/photos/2067627/pexels-photo-2067627.jpeg", Category = "Anniversary", Color = "Pink" },
                new Product { Id = 7, Name = "Blushing Dahlia Dreams", Price = 76m, ImageUrl = "https://images.unsplash.com/photo-1457089328109-e5d9bd499191?auto=format&fit=crop&w=800&q=80", Category = "Wedding", Color = "Pink" },
                new Product { Id = 8, Name = "Sunlit Wildflower Basket", Price = 70m, ImageUrl = "https://images.pexels.com/photos/7382844/pexels-photo-7382844.jpeg", Category = "Birthday", Color = "Yellow" },
                new Product { Id = 9, Name = "Orchid Cascade Luxe", Price = 92m, ImageUrl = "https://images.pexels.com/photos/34749326/pexels-photo-34749326.jpeg", Category = "Wedding", Color = "Purple" },
                new Product { Id = 10, Name = "Lavender Meadow Mix", Price = 66m, ImageUrl = "https://images.pexels.com/photos/207518/pexels-photo-207518.jpeg", Category = "Thank You", Color = "Purple" },
                new Product { Id = 11, Name = "Pastel Hydrangea Box", Price = 78m, ImageUrl = "https://images.pexels.com/photos/14955886/pexels-photo-14955886.jpeg", Category = "Sympathy", Color = "Blue" },
                new Product { Id = 12, Name = "Violet Garden Posy", Price = 63m, ImageUrl = "https://images.pexels.com/photos/1381679/pexels-photo-1381679.jpeg", Category = "Thank You", Color = "Purple" },
                new Product { Id = 13, Name = "Golden Hour Blossoms", Price = 75m, ImageUrl = "https://images.pexels.com/photos/1405699/pexels-photo-1405699.jpeg", Category = "Birthday", Color = "Yellow" },
                new Product { Id = 14, Name = "Rosewater Gift Crate", Price = 69m, ImageUrl = "https://images.pexels.com/photos/20865664/pexels-photo-20865664.jpeg", Category = "Thank You", Color = "Pink" },
                new Product { Id = 15, Name = "Cherry Blossom Spray", Price = 88m, ImageUrl = "https://images.pexels.com/photos/2123755/pexels-photo-2123755.jpeg", Category = "Wedding", Color = "Pink" },
                new Product { Id = 16, Name = "Magnolia Signature Arrangement", Price = 95m, ImageUrl = "https://images.pexels.com/photos/6044630/pexels-photo-6044630.jpeg", Category = "Sympathy", Color = "White" },
                new Product { Id = 17, Name = "Botanical Garden Luxe", Price = 109m, ImageUrl = "https://images.pexels.com/photos/5501101/pexels-photo-5501101.jpeg", Category = "Anniversary", Color = "Green" },
                new Product { Id = 18, Name = "Eucalyptus Rose Harmony", Price = 82m, ImageUrl = "https://images.pexels.com/photos/16961730/pexels-photo-16961730.jpeg", Category = "Wedding", Color = "Mixed" },
                new Product { Id = 19, Name = "Starlit Lily Collection", Price = 90m, ImageUrl = "https://images.pexels.com/photos/10679724/pexels-photo-10679724.jpeg", Category = "Sympathy", Color = "White" },
                new Product { Id = 20, Name = "Crimson Peony Statement", Price = 84m, ImageUrl = "https://images.pexels.com/photos/10533594/pexels-photo-10533594.jpeg", Category = "Anniversary", Color = "Red" },
                new Product { Id = 21, Name = "White Garden Symphony", Price = 77m, ImageUrl = "https://images.pexels.com/photos/17858332/pexels-photo-17858332.jpeg", Category = "Sympathy", Color = "White" }
            );
        }
    }
}

