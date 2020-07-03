using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WebshopDemo.Sales.Commands.AddItemToCart;
using WebshopDemo.Sales.Queries.ActiveCart;
using WebshopDemo.Website.Models;

namespace WebshopDemo.Website.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator mediator;

        public CartController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var activeCart = await mediator.Send(new ActiveCartQuery());

            return View(activeCart);
        }

        public async Task<IActionResult> Add(Guid productID)
        {
            var activeCart = await mediator.Send(new ActiveCartQuery());

            var addItemCommand = new AddItemToCartCommand { CartID = activeCart.CartID, ProductID = productID };
            await mediator.Send(addItemCommand);

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
