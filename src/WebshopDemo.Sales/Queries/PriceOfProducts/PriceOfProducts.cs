using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebshopDemo.Sales.Queries.PriceOfProducts
{
    public class PriceOfProducts
    {
        public Dictionary<Guid, Price> Products { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PriceOfProducts pop &&
                Products.Count == pop.Products.Count && !Products.Except(pop.Products).Any();
        }

        public class Price
        {
            public decimal Amount { get; set; }
            public string Currency { get; set; }

            public override bool Equals(object obj)
            {
                return obj is Price price &&
                       Amount == price.Amount &&
                       Currency == price.Currency;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Amount, Currency);
            }
        }
    }
}
