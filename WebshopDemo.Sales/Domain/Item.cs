using System;
using System.Collections.Generic;

namespace WebshopDemo.Sales.Domain
{
    public class Item
    {
        private Item() { }

        public Item(Guid productID, Price price) : this(productID, price, 1) { }

        public Item(Guid productID, Price price, int quantity)
        {
            ProductID = productID;
            CurrentPrice = price;
            Quantity = quantity;
        }

        public int ID { get; private set; }
        public Guid ProductID { get; private set; }
        public Price CurrentPrice { get; private set; }
        public int Quantity { get; private set; }

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
