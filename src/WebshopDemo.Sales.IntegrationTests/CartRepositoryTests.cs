using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales.IntegrationTests
{
    public class CartRepositoryTests
    {
        private CartRepositoryImp repo;

        [SetUp]
        public void Setup()
        {
            var configBuilder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configBuilder.GetConnectionString("SalesConnection");
            repo = new CartRepositoryImp(connectionString);
        }

        [Test]
        public void AddShouldWork()
        {
            var cartID = Guid.NewGuid();
            var pId = Guid.NewGuid();
            var cart = new Cart(cartID) { Items = new List<Item> { new Item(pId, new Price(5.0m, "EUR")) } };
            repo.Add(cart);
            var rehydratedCart = repo.GetByID(cartID);

            rehydratedCart.State.Should().Be(cart.State);
            rehydratedCart.Should().Be(cart);
            rehydratedCart.Items.Count.Should().Be(cart.Items.Count);
            rehydratedCart.Items.First().Should().Be(cart.Items.First());
        }

        [Test]
        public void SaveShouldWork()
        {
            var cartID = Guid.NewGuid();
            var productID = Guid.NewGuid();
            var productID2 = Guid.NewGuid();
            var cart = new Cart(cartID) 
            { 
                Items = new List<Item> 
                { 
                    new Item(productID, new Price(5.0m, "EUR")),
                    new Item(productID2, new Price(10.0m, "EUR")),
                } };
            repo.Add(cart);
            cart.AddItem(new Item(productID, new Price(5.0m, "EUR")));
            repo.Save(cart);
            var rehydratedCart = repo.GetByID(cartID);

            rehydratedCart.Items.Count.Should().Be(2);
            rehydratedCart.Items.First(x => x.ProductID == productID).Quantity.Should().Be(2);
        }

        [Test]
        public void GetCartsContaingProductsShouldWork()
        {
            var cartID1 = Guid.NewGuid();
            var cartID2 = Guid.NewGuid();
            var productID1 = Guid.NewGuid();
            var productID2 = Guid.NewGuid();
            var cart1 = new Cart(cartID1)
            {
                Items = new List<Item> { new Item(productID1, new Price(10m, "EUR")) }
            };
            var cart2 = new Cart(cartID2)
            {
                Items = new List<Item> { new Item(productID2, new Price(5m, "EUR")) }
            };
            repo.Add(cart1);
            repo.Add(cart2);

            var rehydratedCartList = repo.GetCartsContainingProduct(productID1);

            rehydratedCartList.Count.Should().Be(1);
            rehydratedCartList.First().Should().Be(cart1);
        }
    }
}