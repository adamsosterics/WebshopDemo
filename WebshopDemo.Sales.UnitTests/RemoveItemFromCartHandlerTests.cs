using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Commands.RemoveItemFromCart;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales.UnitTests
{
    [TestFixture]
    class RemoveItemFromCartHandlerTests
    {
        private Mock<CartRepository> cartRepo;
        private RemoveItemFromCartHandler handler;

        [SetUp]
        public void Init()
        {
            cartRepo = new Mock<CartRepository>();

            handler = new RemoveItemFromCartHandler(cartRepo.Object);
        }

        [Test]
        public async Task ShouldRemoveItemFromCart()
        {
            Cart savedCart = null;
            var cartID = Guid.NewGuid();
            var pID = Guid.NewGuid();

            cartRepo.Setup(x => x.GetByID(cartID)).Returns(new Cart(cartID) { Items = new List<Item> { new Item(pID, new Price(10m, "EUR")) } });
            cartRepo.Setup(x => x.Save(It.IsAny<Cart>())).Callback<Cart>(x => savedCart = x);

            await handler.Handle(new RemoveItemFromCartCommand { CartID = cartID, ProductID = pID }, CancellationToken.None);

            cartRepo.Verify(x => x.Save(It.IsAny<Cart>()), Times.Once);

            savedCart.Id.Should().Be(cartID);
            savedCart.Items.Count.Should().Be(0);
        }
    }
}
