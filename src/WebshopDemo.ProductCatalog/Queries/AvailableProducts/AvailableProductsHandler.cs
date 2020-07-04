using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebshopDemo.ProductCatalog.Queries.AvailableProducts
{
    class AvailableProductsHandler : IRequestHandler<AvailableProductsQuery, AvailableProducts>
    {
        private readonly ProductCatalogContext db;

        public AvailableProductsHandler(ProductCatalogContext db)
        {
            this.db = db;
        }

        public async Task<AvailableProducts> Handle(AvailableProductsQuery request, CancellationToken cancellationToken)
        {
            var result = new AvailableProducts();
            result.Products = await db.Products.Select(x => new AvailableProducts.Product { Id = x.Id, Name = x.Name, Description = x.Description }).ToListAsync();
            return result;
        }
    }
}
