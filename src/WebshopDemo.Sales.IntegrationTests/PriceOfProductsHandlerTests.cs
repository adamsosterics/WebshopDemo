using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Queries.PriceOfProducts;

namespace WebshopDemo.Sales.IntegrationTests
{
    [TestFixture]
    class PriceOfProductsHandlerTests
    {
        [Test]
        public async Task ShouldReturnPriceOfGivenProducts()
        {
            var configBuilder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configBuilder.GetConnectionString("SalesConnection");
            var optionsBuilder = new DbContextOptionsBuilder<SalesContext>().UseSqlServer(connectionString);
            var db = new SalesContext(optionsBuilder.Options);

            var productID1 = Guid.NewGuid();
            var productID2 = Guid.NewGuid();

            db.Products.Add(new Domain.Product(productID1) { Price = new Domain.Price(10m, "EUR") });
            db.Products.Add(new Domain.Product(productID2) { Price = new Domain.Price(7.5m, "EUR") });
            db.SaveChanges();

            var handler = new PriceOfProductsHandler(db);
            var query = new PriceOfProductsQuery { ProductIDs = new List<Guid> { productID1, productID2 } };

            var expected = new PriceOfProducts
            {
                Products = new Dictionary<Guid, PriceOfProducts.Price>
                {
                    { productID1, new PriceOfProducts.Price { Amount = 10m, Currency = "EUR" } },
                    { productID2, new PriceOfProducts.Price { Amount = 7.5m, Currency = "EUR" } },
                }
            };

            var response = await handler.Handle(query, CancellationToken.None);

            response.Should().Be(expected);
        }
    }
}
