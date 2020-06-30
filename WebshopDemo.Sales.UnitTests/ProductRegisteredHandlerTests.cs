using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Domain;
using WebshopDemo.Sales.EventHandlers;

namespace WebshopDemo.Sales.UnitTests
{
    public class ProductRegisteredHandlerTests
    {
        [Test]
        public async Task ShouldAddNewProduct()
        {
            var productID = Guid.NewGuid();
            var mediatorMock = new Mock<IMediator>();
            var repoMock = new Mock<ProductRepository>();
            var handler = new ProductRegisteredHandler(mediatorMock.Object, repoMock.Object);

            var product = new Product(productID);

            await handler.Handle(new ProductCatalog.Events.ProductRegistered { ProductID = productID }, CancellationToken.None);

            repoMock.Verify(x => x.Add(product), Times.Once);
        }
    }
}