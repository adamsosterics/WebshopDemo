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
        private Mock<CartRepository> cartRepo;
        private Mock<ProductRepository> productRepo;
        private AddItemToCartHandler handler;

        [SetUp]
        public void Init()
        {
            cartRepo = new Mock<CartRepository>();
            productRepo = new Mock<ProductRepository>();

            handler = new AddItemToCartHandler(cartRepo.Object, productRepo.Object);
        }

        [Test]
        public async Task ShouldAddExistingItemToCart()
        {
            Cart savedCart = null;
            var cartID = Guid.NewGuid();
            var pID = Guid.NewGuid();
            var price = new Price(10m, "EUR");

            cartRepo.Setup(x => x.GetByID(cartID)).Returns(new Cart(cartID));
            cartRepo.Setup(x => x.Save(It.IsAny<Cart>())).Callback<Cart>(x => savedCart = x);

            productRepo.Setup(x => x.GetByID(pID)).Returns(new Product(pID) { Price = price });

            var cart = new Cart(cartID);
            cart.Items.Add(new Item(pID, price));

            await handler.Handle(new AddItemToCartCommand { CartID = cartID, ProductID = pID }, CancellationToken.None);

            cartRepo.Verify(x => x.Save(cart), Times.Once);

            savedCart.Should().Be(cart);
            savedCart.Items.Count.Should().Be(1);
            var item = savedCart.Items.First();
            item.ProductID.Should().Be(pID);
            item.CurrentPrice.Should().Be(price);
        }

        [Test]
        public void ShouldThrowExceptionWhenAddingNonexistingItem()
        {
            Cart savedCart = null;
            var cartID = Guid.NewGuid();
            var pID = Guid.NewGuid();

            cartRepo.Setup(x => x.GetByID(cartID)).Returns(new Cart(cartID));
            cartRepo.Setup(x => x.Save(It.IsAny<Cart>())).Callback<Cart>(x => savedCart = x);

            var exception = new ProductNotFoundException();
            productRepo.Setup(x => x.GetByID(pID)).Throws(exception);

            handler.Invoking(async x => await x.Handle(new AddItemToCartCommand { CartID = cartID, ProductID = pID }, CancellationToken.None))
                .Should()
                .Throw<AddItemToCartException>()
                .Where(x => (Guid)x.Data["ProductID"] == pID && (Guid)x.Data["CartID"] == cartID)
                .WithInnerException<ProductNotFoundException>();
        }

        [Test]
        public async Task ShouldBeAbleToAddSameProductMultipleTimes()
        {
            Cart savedCart = null;
            var cartID = Guid.NewGuid();
            var pID = Guid.NewGuid();
            var price = new Price(10m, "EUR");

            cartRepo.Setup(x => x.GetByID(cartID)).Returns(new Cart(cartID) { Items = new List<Item> { new Item(pID, price) } });
            cartRepo.Setup(x => x.Save(It.IsAny<Cart>())).Callback<Cart>(x => savedCart = x);

            productRepo.Setup(x => x.GetByID(pID)).Returns(new Product(pID) { Price = price });

            var cart = new Cart(cartID);
            cart.Items.Add(new Item(pID, price, 2));

            await handler.Handle(new AddItemToCartCommand { CartID = cartID, ProductID = pID }, CancellationToken.None);

            cartRepo.Verify(x => x.Save(cart), Times.Once);

            savedCart.Should().Be(cart);
            savedCart.Items.Count.Should().Be(1);
            var item = savedCart.Items.First();
            item.ProductID.Should().Be(pID);
            item.CurrentPrice.Should().Be(price);
            item.Quantity.Should().Be(2);
        }
    }
}
