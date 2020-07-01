using System;
using System.Collections.Generic;

namespace WebshopDemo.Sales.Domain
{
    public class Item
    {
        public Item(Guid productID, Price price) : this(productID, price, 1) { }

        public Item(Guid productID, Price price, int quantity)
        {
            ProductID = productID;
            CurrentPrice = price;
            Quantity = quantity;
        }

        public Guid ProductID { get; }
        public Price CurrentPrice { get; }
        public int Quantity { get; }

        public Item AddQuantity(int quantity)
        {
            return new Item(ProductID, CurrentPrice, Quantity + quantity);
        }

        public override bool Equals(object obj)
        {
            return obj is Item item &&
                   ProductID.Equals(item.ProductID) &&
                   EqualityComparer<Price>.Default.Equals(CurrentPrice, item.CurrentPrice) &&
                   Quantity == item.Quantity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductID, CurrentPrice, Quantity);
        }
    }
}
