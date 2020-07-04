using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales.Commands.RemoveItemFromCart
{
    public class RemoveItemFromCartHandler : IRequestHandler<RemoveItemFromCartCommand>
    {
        private readonly CartRepository cartRepo;

        public RemoveItemFromCartHandler(CartRepository cartRepo)
        {
            this.cartRepo = cartRepo;
        }

        public Task<Unit> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
        {
            var cart = cartRepo.GetByID(request.CartID);
            cart.RemoveItem(request.ProductID);
            cartRepo.Save(cart);
            return Task.FromResult(new Unit());
        }
    }
}
