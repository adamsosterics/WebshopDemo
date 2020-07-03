using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Domain;
using WebshopDemo.Sales.EventHandlers;

namespace WebshopDemo.Sales.UnitTests
{
    [TestFixture]
    class PriceChangedHandlerTests
    {
        [Test]
        public async Task ShouldChangePriceOfProductInCarts()
        {
            var productRepoMock = new Mock<ProductRepository>();
            var cartRepoMock = new Mock<CartRepository>();
            var handler = new PriceChangedHandler(productRepoMock.Object, cartRepoMock.Object);

            var newPrice = new Price(10m, "EUR");
            var oldPrice = new Price(5m, "EUR");
            var productID = Guid.NewGuid();
            var cartID = Guid.NewGuid();

            Cart savedCart = null;
            productRepoMock.Setup(x => x.GetByID(productID)).Returns(new Product(productID) { Price = newPrice });
            cartRepoMock.Setup(x => x.GetCartsContainingProduct(productID))
                .Returns(new List<Cart> { new Cart(cartID) { State = CartState.Active, Items = new List<Item> { new Item(productID, oldPrice) } } });
            cartRepoMock.Setup(x => x.Save(It.IsAny<Cart>())).Callback<Cart>(x => savedCart = x);

            await handler.Handle(new Events.PriceChanged { ProductID = productID }, CancellationToken.None);

            var item = savedCart.Items.First();
            item.CurrentPrice.Should().Be(newPrice);
            item.LastPrice.Should().Be(oldPrice);
        }
    }
}
