using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebshopDemo.Sales.Queries.ActiveCart
{
    public class ActiveCartHandler : IRequestHandler<ActiveCartQuery, ActiveCart>
    {
        private readonly ProductRepositoryImp db;

        public ActiveCartHandler(ProductRepositoryImp db)
        {
            this.db = db;
        }

        public Task<ActiveCart> Handle(ActiveCartQuery request, CancellationToken cancellationToken)
        {
            var activeCart = new ActiveCart();
            activeCart.CartID = db.Carts.First(x => x.State == Domain.CartState.Active).Id;
            return Task.FromResult(activeCart);
        }
    }
}
