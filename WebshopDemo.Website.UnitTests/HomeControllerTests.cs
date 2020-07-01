using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.ProductCatalog.Queries.AvailableProducts;
using WebshopDemo.Sales.Queries.PriceOfProducts;
using WebshopDemo.Website.Controllers;
using WebshopDemo.Website.Models;

namespace WebshopDemo.Website.UnitTests
{
    public class HomeControllerTests
    {
        [Test]
        public async Task IndexGetShouldReturnProductsWithPrices()
        {
            var loggerMock = new Mock<ILogger<HomeController>>();
            var mediatorMock = new Mock<IMediator>();

            var pID1 = Guid.NewGuid();
            var name1 = "name1";
            var desc1 = "desc1";
            var am1 = 10m;
            var cur1 = "EUR";
            var pID2 = Guid.NewGuid();
            var name2 = "name2";
            var desc2 = "desc2";
            var am2 = 7.5m;
            var cur2 = "USD";

            mediatorMock.Setup(x => x.Send(It.IsAny<AvailableProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new AvailableProducts
                    {
                        Products = new List<AvailableProducts.Product>
                        {
                            new AvailableProducts.Product
                            {
                                Id = pID1,
                                Name = name1,
                                Description = desc1
                            },
                            new AvailableProducts.Product
                            {
                                Id = pID2,
                                Name = name2,
                                Description = desc2
                            },
                        }
                    });
            mediatorMock.Setup(x => x.Send(It.IsAny<PriceOfProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new PriceOfProducts
                    {
                        Products = new Dictionary<Guid, PriceOfProducts.Price>
                        {
                            { pID1, new PriceOfProducts.Price { Amount = am1, Currency = cur1 } },
                            { pID2, new PriceOfProducts.Price { Amount = am2, Currency = cur2 } },
                        }
                    }
                );

            var homeController = new HomeController(loggerMock.Object, mediatorMock.Object);
            var result = await homeController.Index();
            var productsVM = (ProductsModel)(result as ViewResult).Model;

            productsVM.Products.Count.Should().Be(2);
            var p1 = productsVM.Products.First(x => x.ProductID == pID1);
            p1.Price.Amount.Should().Be(am1);
            p1.Price.Currency.Should().Be(cur1);
            p1.Name.Should().Be(name1);
            p1.Description.Should().Be(desc1);
            var p2 = productsVM.Products.First(x => x.ProductID == pID2);
            p2.Price.Amount.Should().Be(am2);
            p2.Price.Currency.Should().Be(cur2);
            p2.Name.Should().Be(name2);
            p2.Description.Should().Be(desc2);
        }

        [Test]
        public async Task IndexGetShouldNotReturnProductsWithoutPrices()
        {
            var loggerMock = new Mock<ILogger<HomeController>>();
            var mediatorMock = new Mock<IMediator>();

            var pID1 = Guid.NewGuid();
            var name1 = "name1";
            var desc1 = "desc1";
            var am1 = 10m;
            var cur1 = "EUR";
            var pID2 = Guid.NewGuid();
            var name2 = "name2";
            var desc2 = "desc2";

            mediatorMock.Setup(x => x.Send(It.IsAny<AvailableProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new AvailableProducts
                    {
                        Products = new List<AvailableProducts.Product>
                        {
                            new AvailableProducts.Product
                            {
                                Id = pID1,
                                Name = name1,
                                Description = desc1
                            },
                            new AvailableProducts.Product
                            {
                                Id = pID2,
                                Name = name2,
                                Description = desc2
                            },
                        }
                    });
            mediatorMock.Setup(x => x.Send(It.IsAny<PriceOfProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new PriceOfProducts
                    {
                        Products = new Dictionary<Guid, PriceOfProducts.Price>
                        {
                            { pID1, new PriceOfProducts.Price { Amount = am1, Currency = cur1 } },
                        }
                    }
                );

            var homeController = new HomeController(loggerMock.Object, mediatorMock.Object);
            var result = await homeController.Index();
            var productsVM = (ProductsModel)(result as ViewResult).Model;

            productsVM.Products.Count.Should().Be(1);
            var p1 = productsVM.Products.First(x => x.ProductID == pID1);
            p1.Price.Amount.Should().Be(am1);
            p1.Price.Currency.Should().Be(cur1);
            p1.Name.Should().Be(name1);
            p1.Description.Should().Be(desc1);
        }
    }
}