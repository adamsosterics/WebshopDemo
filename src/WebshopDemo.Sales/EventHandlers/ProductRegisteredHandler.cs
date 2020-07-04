using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.ProductCatalog.Events;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales.EventHandlers
{
    public class ProductRegisteredHandler : INotificationHandler<ProductRegistered>
    {
        private readonly ProductRepository productRepository;

        public ProductRegisteredHandler(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public Task Handle(ProductRegistered notification, CancellationToken cancellationToken)
        {
            productRepository.Add(new Product(notification.ProductID));
            return Task.CompletedTask;
        }
    }
}
