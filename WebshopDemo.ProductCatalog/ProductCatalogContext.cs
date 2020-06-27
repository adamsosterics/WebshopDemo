using Microsoft.EntityFrameworkCore;
using WebshopDemo.ProductCatalog.Domain;

namespace WebshopDemo.ProductCatalog
{
    public class ProductCatalogContext : DbContext
    {
        public ProductCatalogContext()
        {
        }

        public ProductCatalogContext(DbContextOptions<ProductCatalogContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
