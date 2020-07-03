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
            foreach (var item in cart.Items)
            {
                var acItem = new ActiveCart.Item();
                acItem.ProductID = item.ProductID;
                acItem.CurrentPrice = new ActiveCart.Price { Amount = item.CurrentPrice.Amount, Currency = item.CurrentPrice.Currency };
                if (item.LastPrice != null)
                {
                    acItem.LastPrice = new ActiveCart.Price { Amount = item.LastPrice.Amount, Currency = item.LastPrice.Currency };
                }
                acItem.Quantity = item.Quantity;
                activeCart.Items.Add(acItem);
            }
            return Task.FromResult(activeCart);
        }
    }
}
