using System;
using System.Collections.Generic;

namespace WebshopDemo.Sales.Domain
{
    public class Item
    {
        public Item(Guid productID, Price price)
        {
            ProductID = productID;
            CurrentPrice = price;
        }

        public Guid ProductID { get; }
        public Price CurrentPrice { get; }

        public override bool Equals(object obj)
        {
            return obj is Item item &&
                   ProductID.Equals(item.ProductID) &&
                   EqualityComparer<Price>.Default.Equals(CurrentPrice, item.CurrentPrice);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductID, CurrentPrice);
        }
    }
}
