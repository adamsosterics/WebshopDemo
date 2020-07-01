using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Commands.AddItemToCart;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales.UnitTests
{
    [TestFixture]
    class AddItemToCartHandlerTests
    {
        [Test]
        public async Task ShouldAddExistingItemToCart()
        {
            var cartRepo = new Mock<CartRepository>();
            var productRepo = new Mock<ProductRepository>();

            Cart savedCart = null;
            var cartID = Guid.NewGuid();
            var pID = Guid.NewGuid();
            var price = new Price(10m, "EUR");

            cartRepo.Setup(x => x.GetByID(cartID)).Returns(new Cart(cartID));
            cartRepo.Setup(x => x.Save(It.IsAny<Cart>())).Callback<Cart>(x => savedCart = x);

            productRepo.Setup(x => x.GetByID(pID)).Returns(new Product(pID) { Price = price });

            var cart = new Cart(cartID);
            cart.Items.Add(pID, new Item(pID, price));

            var handler = new AddItemToCartHandler(cartRepo.Object, productRepo.Object);

            await handler.Handle(new AddItemToCartCommand { CartID = cartID, ProductID = pID }, CancellationToken.None);

            cartRepo.Verify(x => x.Save(cart), Times.Once);

            savedCart.Should().Be(cart);
            savedCart.Items.Count.Should().Be(1);
            var item = savedCart.Items.First();
            item.Value.ProductID.Should().Be(pID);
            item.Value.CurrentPrice.Should().Be(price);
        }

        [Test]
        public void ShouldThrowExceptionWhenAddingNonexistingItem()
        {
            var cartRepo = new Mock<CartRepository>();
            var productRepo = new Mock<ProductRepository>();

            Cart savedCart = null;
            var cartID = Guid.NewGuid();
            var pID = Guid.NewGuid();

            cartRepo.Setup(x => x.GetByID(cartID)).Returns(new Cart(cartID));
            cartRepo.Setup(x => x.Save(It.IsAny<Cart>())).Callback<Cart>(x => savedCart = x);

            var exception = new ProductNotFoundException();
            productRepo.Setup(x => x.GetByID(pID)).Throws(exception);

            var handler = new AddItemToCartHandler(cartRepo.Object, productRepo.Object);

            handler.Invoking(async x => await x.Handle(new AddItemToCartCommand { CartID = cartID, ProductID = pID }, CancellationToken.None))
                .Should()
                .Throw<AddItemToCartException>()
                .Where(x => (Guid)x.Data["ProductID"] == pID && (Guid)x.Data["CartID"] == cartID)
                .WithInnerException<ProductNotFoundException>();
        }

        [Test]
        public async Task ShouldBeAbleToAddSameProductMultipleTimes()
        {
            var cartRepo = new Mock<CartRepository>();
            var productRepo = new Mock<ProductRepository>();

            Cart savedCart = null;
            var cartID = Guid.NewGuid();
            var pID = Guid.NewGuid();
            var price = new Price(10m, "EUR");

            cartRepo.Setup(x => x.GetByID(cartID)).Returns(new Cart(cartID) { Items = new Dictionary<Guid, Item> { { pID, new Item(pID, price) } } });
            cartRepo.Setup(x => x.Save(It.IsAny<Cart>())).Callback<Cart>(x => savedCart = x);

            productRepo.Setup(x => x.GetByID(pID)).Returns(new Product(pID) { Price = price });

            var cart = new Cart(cartID);
            cart.Items.Add(pID, new Item(pID, price, 2));

            var handler = new AddItemToCartHandler(cartRepo.Object, productRepo.Object);

            await handler.Handle(new AddItemToCartCommand { CartID = cartID, ProductID = pID }, CancellationToken.None);

            cartRepo.Verify(x => x.Save(cart), Times.Once);

            savedCart.Should().Be(cart);
            savedCart.Items.Count.Should().Be(1);
            var item = savedCart.Items.First();
            item.Value.ProductID.Should().Be(pID);
            item.Value.CurrentPrice.Should().Be(price);
            item.Value.Quantity.Should().Be(2);
        }
    }
}
