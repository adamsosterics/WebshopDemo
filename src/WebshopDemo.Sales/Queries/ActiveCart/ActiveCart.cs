﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace WebshopDemo.Sales.Queries.ActiveCart
{
    public class ActiveCart
    {
        public Guid CartID { get; set; }
        public List<Item> Items { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ActiveCart cart &&
                   CartID.Equals(cart.CartID) &&
                   Items.SequenceEqual(cart.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CartID);
        }

        public class Item
        {
            public Guid ProductID { get; set; }
            public int Quantity { get; set; }
            public Price CurrentPrice { get; set; }
            public Price LastPrice { get; set; }

            public override bool Equals(object obj)
            {
                return obj is Item item &&
                       ProductID.Equals(item.ProductID) &&
                       Quantity == item.Quantity &&
                       EqualityComparer<Price>.Default.Equals(CurrentPrice, item.CurrentPrice) &&
                       EqualityComparer<Price>.Default.Equals(LastPrice, item.LastPrice);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(ProductID, Quantity, CurrentPrice, LastPrice);
            }
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
