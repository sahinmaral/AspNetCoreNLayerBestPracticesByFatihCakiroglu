using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
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

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity entityReference)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            Entry(entityReference).Property(x => x.UpdatedDate).IsModified = false;

                            entityReference.CreatedDate = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;

                            entityReference.UpdatedDate = DateTime.Now;
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if(entry.Entity is BaseEntity entityReference)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            Entry(entityReference).Property(x => x.UpdatedDate).IsModified = false;

                            entityReference.CreatedDate= DateTime.Now;
                            break;
                        case EntityState.Modified:
                            Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;

                            entityReference.UpdatedDate= DateTime.Now;
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
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
