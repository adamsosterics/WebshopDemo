using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales
{
    public class SalesContext : DbContext, ProductRepository
    {
        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions<SalesContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }

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

            modelBuilder.Entity<Cart>().HasKey(c => c.Id);
            modelBuilder.Entity<Cart>().HasMany(c => c.Items).WithOne();

            modelBuilder.Entity<Item>().HasKey(i => i.ID);
            modelBuilder.Entity<Item>().OwnsOne(i => i.CurrentPrice);
            modelBuilder.Entity<Item>().ToTable("Items");

            base.OnModelCreating(modelBuilder);
        }
    }
}
