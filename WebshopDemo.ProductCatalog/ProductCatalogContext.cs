using Microsoft.EntityFrameworkCore;
using WebshopDemo.ProductCatalog.Domain;

namespace WebshopDemo.ProductCatalog
{
    class ProductCatalogContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSqlLocalDB;Initial Catalog=WebshopDemo_ProductCatalog;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
