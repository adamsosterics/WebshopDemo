using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales.IntegrationTests
{
    public class ProductRepositoryImpTests
    {
        private SalesContext repo;

        [SetUp]
        public void Setup()
        {
            var configBuilder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configBuilder.GetConnectionString("SalesConnection");
            var optionsBuilder = new DbContextOptionsBuilder<SalesContext>().UseSqlServer(connectionString);
            repo = new SalesContext(optionsBuilder.Options);
            repo.Database.ExecuteSqlRaw("TRUNCATE TABLE Products");
        }

        [Test]
        public void AddShouldWork()
        {
            var productID = Guid.NewGuid();
            var product = new Product(productID) { Price = new Price(5.0m, "EUR") };
            repo.Add(product);
            var rehydratedProduct = repo.GetByID(productID);

            rehydratedProduct.Price.Should().Be(product.Price);
            rehydratedProduct.Should().Be(product);
        }

        [Test]
        public void GetByIDShouldThrowExceptionWhenProductDoesntExist()
        {
            var productID = Guid.NewGuid();
            repo.Invoking(x => x.GetByID(productID)).Should().Throw<ProductNotFoundException>();
        }

        [Test]
        public void SaveShouldWork()
        {
            var productID = Guid.NewGuid();
            var product = new Product(productID) { Price = new Price(5.0m, "EUR") };
            repo.Add(product);
            product.SetPrice(new Price(10m, "USD"));
            repo.Save(product);
            var rehydratedProduct = repo.GetByID(productID);

            rehydratedProduct.Price.Should().Be(product.Price);
            rehydratedProduct.Should().Be(product);
        }
    }
}