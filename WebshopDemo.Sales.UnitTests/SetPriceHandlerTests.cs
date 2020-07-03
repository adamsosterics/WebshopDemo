using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Commands.SetPrice;
using WebshopDemo.Sales.Domain;
using WebshopDemo.Sales.Events;

namespace WebshopDemo.Sales.UnitTests
{
    [TestFixture]
    class SetPriceHandlerTests
    {
        private SetPriceHandler handler;
        private Mock<ProductRepository> repoMock;
        private Mock<IMediator> mediatorMock;

        [SetUp]
        public void Init()
        {
            repoMock = new Mock<ProductRepository>();
            mediatorMock = new Mock<IMediator>();
            handler = new SetPriceHandler(repoMock.Object, mediatorMock.Object);
        }

        [Test]
        public async Task ShouldSetPriceOfProduct()
        {
            var productID = Guid.NewGuid();
            var amount = 5m;
            var currency = "EUR";
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

        [Test]
        public async Task ShouldRaisePriceChangedEvent()
        {
            var productID = Guid.NewGuid();
            var amount = 5m;
            var currency = "EUR";

            PriceChanged e = null;

            repoMock.Setup(x => x.GetByID(productID)).Returns(new Product(productID) { Price = new Price(1m, "EUR") });
            mediatorMock.Setup(x => x.Send(It.IsAny<PriceChanged>(), It.IsAny<CancellationToken>())).Callback<object, CancellationToken>((o, ct) => e = (PriceChanged)o);
            await handler.Handle(new SetPriceCommand { ProductID = productID, ProductPrice = new SetPriceCommand.Price { Amount = amount, Currency = currency } }, CancellationToken.None);

            e.ProductID.Should().Be(productID);
        }
    }
}
