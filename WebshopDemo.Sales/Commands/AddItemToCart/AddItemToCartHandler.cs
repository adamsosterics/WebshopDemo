using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales.Commands.AddItemToCart
{
    public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand>
    {
        private readonly CartRepository cartRepo;
        private readonly ProductRepository productRepo;

        public AddItemToCartHandler(CartRepository cartRepo, ProductRepository productRepo)
        {
            this.cartRepo = cartRepo;
            this.productRepo = productRepo;
        }

        public Task<Unit> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            var cart = cartRepo.GetByID(request.CartID);
            var product = productRepo.GetByID(request.ProductID);
            cart.AddItem(new Item(request.ProductID, product.Price));
            cartRepo.Save(cart);
            return Task.FromResult(new Unit());
        }
    }
}
