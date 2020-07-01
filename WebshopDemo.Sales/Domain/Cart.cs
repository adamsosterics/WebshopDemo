using System;
using System.Collections.Generic;

namespace WebshopDemo.Sales.Domain
{
    public class Cart
    {
        public Cart(Guid id)
        {
            Id = id;
            Items = new Dictionary<Guid, Item>();
        }

        public Guid Id { get; }
        public CartState State { get; internal set; }
        public Dictionary<Guid, Item> Items { get; internal set; }

        public void AddItem(Item item)
        {
            if (Items.ContainsKey(item.ProductID))
            {
                Items[item.ProductID] = Items[item.ProductID].AddQuantity(item.Quantity);
            }
            else
            {
                Items.Add(item.ProductID, item);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Cart cart &&
                   Id.Equals(cart.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
