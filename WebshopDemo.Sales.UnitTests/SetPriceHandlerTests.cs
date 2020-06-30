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
        private SetPriceHandler handler;
        private Mock<ProductRepository> repoMock;

        [SetUp]
        public void Init()
        {
            repoMock = new Mock<ProductRepository>();
            handler = new SetPriceHandler(repoMock.Object);
        }

        [Test]
        public async Task ShouldSetPriceOfProduct()
        {
            var productID = Guid.NewGuid();
            var amount = 5m;
            var currency = "euro";
            var product = new Product(productID);
            product.Price = new Price(amount, currency);

            Product savedProduct = null;

            repoMock.Setup(x => x.GetByID(productID)).Returns(new Product(productID));
            repoMock.Setup(x => x.Save(It.IsAny<Product>())).Callback<Product>(x => savedProduct = x);

            await handler.Handle(new SetPriceCommand { ProductID = productID, ProductPrice = new SetPriceCommand.Price { Amount = amount, Currency = currency } }, CancellationToken.None);

            repoMock.Verify(x => x.Save(product), Times.Once);
            savedProduct.Price.Should().Be(product.Price);
        }

        [Test]
        public void ShouldThrowExceptionOnNonexistingProduct()
        {
            var nonExistingProductID = Guid.NewGuid();
            var exception = new ProductNotFoundException();
            repoMock.Setup(x => x.GetByID(nonExistingProductID)).Throws(exception);

            handler.Invoking(async x => 
                await x.Handle(new SetPriceCommand { ProductID = nonExistingProductID, ProductPrice = new SetPriceCommand.Price { Amount = 1m, Currency = "euro" } }, CancellationToken.None))
                .Should()
                .Throw<SetPriceException>()
                .Where(x => (Guid)x.Data["ProductID"] == nonExistingProductID)
                .WithInnerException<ProductNotFoundException>();
        }
    }
}
