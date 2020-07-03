using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebshopDemo.Sales.Queries.ActiveCart
{
    public class ActiveCartHandler : IRequestHandler<ActiveCartQuery, ActiveCart>
    {
        private readonly SalesContext db;

        public ActiveCartHandler(SalesContext db)
        {
            this.db = db;
        }

        public Task<ActiveCart> Handle(ActiveCartQuery request, CancellationToken cancellationToken)
        {
            var activeCart = new ActiveCart();
            var cart = db.Carts.Include(x => x.Items).First(x => x.State == Domain.CartState.Active);
            activeCart.CartID = cart.Id;
            activeCart.Items = new List<ActiveCart.Item>();
            activeCart.Items.AddRange(cart.Items.Select(x => 
                new ActiveCart.Item { ProductID = x.ProductID, Price = new ActiveCart.Price { Amount = x.CurrentPrice.Amount, Currency = x.CurrentPrice.Currency }, Quantity = x.Quantity }));
            return Task.FromResult(activeCart);
        }
    }
}
