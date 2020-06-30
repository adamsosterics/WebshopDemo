using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales
{
    class ProductRepositoryImp : DbContext, ProductRepository
    {
        public ProductRepositoryImp()
        {
        }

        public ProductRepositoryImp(DbContextOptions<ProductRepositoryImp> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public void Add(Product product)
        {
            Products.Add(product);
            SaveChanges();
        }

        public Product GetByID(Guid id)
        {
            var product = Products.Where(x => x.Id == id).FirstOrDefault();
            if (product == null)
            {
                throw new ProductNotFoundException();
            }
            return product;
        }

        public void Save(Product product)
        {
            Products.Update(product);
            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().OwnsOne(x => x.Price);

            base.OnModelCreating(modelBuilder);
        }
    }
}
