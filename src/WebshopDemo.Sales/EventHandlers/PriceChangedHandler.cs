using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Domain;
using WebshopDemo.Sales.Events;

namespace WebshopDemo.Sales.EventHandlers
{
    public class PriceChangedHandler : INotificationHandler<PriceChanged>
    {
        private readonly ProductRepository productRepo;
        private readonly CartRepository cartRepo;

        public PriceChangedHandler(ProductRepository productRepo, CartRepository cartRepo)
        {
            this.productRepo = productRepo;
            this.cartRepo = cartRepo;
        }

        public Task Handle(PriceChanged notification, CancellationToken cancellationToken)
        {
            var newPrice = productRepo.GetByID(notification.ProductID).Price;
            var carts = cartRepo.GetCartsContainingProduct(notification.ProductID);
            foreach (var cart in carts)
            {
                cart.ChangeItemPrice(notification.ProductID, newPrice);
                cartRepo.Save(cart);
            }
            return Task.FromResult(new Unit());
        }
    }
}
