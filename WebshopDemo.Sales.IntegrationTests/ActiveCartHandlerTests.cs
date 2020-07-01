using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
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
            var optionsBuilder = new DbContextOptionsBuilder<SalesContext>().UseSqlServer(connectionString);
            var db = new SalesContext(optionsBuilder.Options);

            var cartID = Guid.NewGuid();

            db.Database.ExecuteSqlRaw("TRUNCATE TABLE Carts");
            db.Carts.Add(new Cart(cartID) { State = CartState.Active });
            db.SaveChanges();

            var handler = new ActiveCartHandler(db);

            var expected = new ActiveCart
            {
                CartID = cartID
            };

            var response = await handler.Handle(new ActiveCartQuery(), CancellationToken.None);

            response.Should().Be(expected);
        }
    }
}
