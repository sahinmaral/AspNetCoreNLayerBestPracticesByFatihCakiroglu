using Microsoft.EntityFrameworkCore;

using NLayer.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        // Program.cs uzerinden veritabanimizin konfigurasyonlarini belirlemek icin
        // DbContextOptions sinifini constructor metot parametresine yaziyoruz
        public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Repository class library uzerindeki configuration lari uygular.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<ProductFeature>().HasData(
                new ProductFeature
                {
                    ProductId = 1,
                    Id = 1,
                    Color = "Kirmizi",
                    Height = 100,
                    Width = 100,
                });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
    }
}
