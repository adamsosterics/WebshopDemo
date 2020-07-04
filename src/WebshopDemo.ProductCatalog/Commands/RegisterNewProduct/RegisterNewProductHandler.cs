using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.ProductCatalog.Domain;
using WebshopDemo.ProductCatalog.Events;

namespace WebshopDemo.ProductCatalog.Commands.RegisterNewProduct
{
    public class RegisterNewProductHandler : IRequestHandler<RegisterNewProductCommand, Guid>
    {
        private readonly IMediator mediator;
        private readonly ProductCatalogContext db;

        public RegisterNewProductHandler(IMediator mediator, ProductCatalogContext db)
        {
            this.mediator = mediator;
            this.db = db;
        }
        public async Task<Guid> Handle(RegisterNewProductCommand request, CancellationToken cancellationToken)
        {
            var newProductID = Guid.NewGuid();
            db.Products.Add(new Product(id: newProductID, name: request.Name, description: request.Description));

            await db.SaveChangesAsync();

            await mediator.Publish(new ProductRegistered { ProductID = newProductID });

            return newProductID;
        }
    }
}
