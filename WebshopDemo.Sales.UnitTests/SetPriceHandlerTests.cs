using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Commands.SetPrice;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales.UnitTests
{
    [TestFixture]
    class SetPriceHandlerTests
    {
        [Test]
        public async Task ShouldSetPriceOfProduct()
        {
            var productID = Guid.NewGuid();
            var amount = 5m;
            var currency = "euro";
            var repoMock = new Mock<ProductRepository>();
            var product = new Product(productID);
            product.Price = new Price(amount, currency);

            Product savedProduct = null;

            repoMock.Setup(x => x.GetByID(productID)).Returns(new Product(productID));
            repoMock.Setup(x => x.Save(It.IsAny<Product>())).Callback<Product>(x => savedProduct = x);

            var handler = new SetPriceHandler(repoMock.Object);

            await handler.Handle(new SetPriceCommand { ProductID = productID, ProductPrice = new SetPriceCommand.Price { Amount = amount, Currency = currency } }, CancellationToken.None);

            repoMock.Verify(x => x.Save(product), Times.Once);
            savedProduct.Price.Should().Be(product.Price);
        }
    }
}
