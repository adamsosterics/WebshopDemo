using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebshopDemo.ProductCatalog.Queries.AvailableProducts;
using WebshopDemo.Sales.Queries.PriceOfProducts;
using WebshopDemo.Website.Models;

namespace WebshopDemo.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator mediator;

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new ProductsModel();

            var availableProducts = await mediator.Send(new AvailableProductsQuery());
            var prices = await mediator.Send(new PriceOfProductsQuery { ProductIDs = availableProducts.Products.Select(x => x.Id).ToList() });

            availableProducts.Products = availableProducts.Products.Where(x => prices.Products.Keys.Contains(x.Id)).ToList();

            vm.Products = new List<ProductsModel.Product>();
            availableProducts.Products.ForEach(x =>
                vm.Products.Add(
                    new ProductsModel.Product
                    {
                        ProductID = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Price = new ProductsModel.Price { Amount = prices.Products[x.Id].Amount, Currency = prices.Products[x.Id].Currency }
                    }));

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
