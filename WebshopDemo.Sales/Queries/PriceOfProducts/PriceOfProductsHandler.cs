using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace WebshopDemo.Sales.Queries.PriceOfProducts
{
    public class PriceOfProductsHandler : IRequestHandler<PriceOfProductsQuery, PriceOfProducts>
    {
        private readonly ProductRepositoryImp db;

        public PriceOfProductsHandler(ProductRepositoryImp db)
        {
            this.db = db;
        }

        public async Task<PriceOfProducts> Handle(PriceOfProductsQuery request, CancellationToken cancellationToken)
        {
            var result = new PriceOfProducts();
            result.Products = new Dictionary<System.Guid, PriceOfProducts.Price>();
            await db.Products
                .Where(x => request.ProductIDs.Contains(x.Id))
                .ForEachAsync(x => result.Products.Add(x.Id, new PriceOfProducts.Price { Amount = x.Price.Amount, Currency = x.Price.Currency }));
            return result;
        }
    }
}
