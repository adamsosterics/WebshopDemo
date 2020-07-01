using System;
using System.Collections.Generic;

namespace WebshopDemo.Sales.Domain
{
    public class Cart
    {
        public Cart(Guid id)
        {
            Id = id;
            Items = new List<Item>();
        }

        public Guid Id { get; }
        public CartState State { get; internal set; }
        public List<Item> Items { get; internal set; }

        public void AddItem(Item item)
        {
            Items.Add(item);
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
