using MediatR;
using System;
using System.Collections.Generic;

namespace WebshopDemo.Sales.Queries.PriceOfProducts
{
    public class PriceOfProductsQuery : IRequest<PriceOfProducts>
    {
        public List<Guid> ProductIDs { get; set; }
    }
}
