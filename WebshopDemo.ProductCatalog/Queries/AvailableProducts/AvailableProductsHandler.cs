using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebshopDemo.ProductCatalog.Queries.AvailableProducts
{
    class AvailableProductsHandler : IRequestHandler<AvailableProductsQuery, AvailableProducts>
    {
        public async Task<AvailableProducts> Handle(AvailableProductsQuery request, CancellationToken cancellationToken)
        {
            using (var db = new ProductCatalogContext())
            {
                var result = new AvailableProducts();
                result.Products = await db.Products.Select(x => new AvailableProducts.Product { Id = x.Id, Name = x.Name, Description = x.Description }).ToListAsync();
                return result;
            }
        }
    }
}
