using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WebshopDemo.ProductCatalog.Commands.RegisterNewProduct;
using WebshopDemo.ProductCatalog.Queries.AvailableProducts;
using WebshopDemo.Sales.Commands.SetPrice;
using WebshopDemo.Website.Models;

namespace WebshopDemo.Website.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMediator mediator;

        public ProductController(ILogger<ProductController> logger, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new AvailableProductsQuery());
            return View(response);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewProductCommand productVM)
        {
            await mediator.Send(productVM);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult SetPrice(Guid id)
        {
            return View(new SetPriceCommand { ProductID = id });
        }

        [HttpPost]
        public async Task<IActionResult> SetPrice(SetPriceCommand vm)
        {
            await mediator.Send(vm);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
