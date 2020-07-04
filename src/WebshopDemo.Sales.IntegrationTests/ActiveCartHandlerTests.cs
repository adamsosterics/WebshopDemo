using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Domain;
using WebshopDemo.Sales.Queries.ActiveCart;

namespace WebshopDemo.Sales.IntegrationTests
{
    [TestFixture]
    class ActiveCartHandlerTests
    {
        [Test]
        public async Task ShouldReturnActiveCart()
        {
            var configBuilder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configBuilder.GetConnectionString("SalesConnection");
            var optionsBuilder = new DbContextOptionsBuilder<SalesContext>().UseSqlServer(connectionString).UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()));
            var db = new SalesContext(optionsBuilder.Options);

            var cartID = Guid.NewGuid();
            var pID = Guid.NewGuid();
            var amount = 10m;
            var currency = "EUR";
            var lastAmount = 5m;

            db.Database.ExecuteSqlRaw("TRUNCATE TABLE Items");
            db.Database.ExecuteSqlRaw("DELETE FROM Carts");
            db.Carts.Add(new Cart(cartID) { State = CartState.Active, Items = new List<Item> { new Item(pID, currentPrice: new Price(amount, currency), lastPrice: new Price(lastAmount, currency), 1)} });
            db.SaveChanges();

            var handler = new ActiveCartHandler(db);

            var expected = new ActiveCart
            {
                CartID = cartID,
                Items = new List<ActiveCart.Item> 
                { 
                    new ActiveCart.Item 
                    { 
                        ProductID = pID, 
                        Quantity = 1, 
                        CurrentPrice = new ActiveCart.Price { Amount = amount, Currency = currency },
                        LastPrice = new ActiveCart.Price { Amount = lastAmount, Currency = currency }
                    } 
                }
            };

            var response = await handler.Handle(new ActiveCartQuery(), CancellationToken.None);

            response.Should().Be(expected);
        }
    }
}
