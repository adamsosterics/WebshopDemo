using System;
using System.Collections.Generic;
using System.Linq;

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
            var itemInList = Items.FirstOrDefault(x => x.ProductID == item.ProductID);
            if (itemInList != null)
            {
                Items.Add(itemInList.AddQuantity(item.Quantity));
                Items.Remove(itemInList);
            }
            else
            {
                Items.Add(item);
            }
        }

        public void RemoveItem(Guid productID)
        {
            Items.RemoveAll(x => x.ProductID == productID);
        }

        public void ChangeItemPrice(Guid productID, Price newPrice)
        {
            var itemInList = Items.First(x => x.ProductID == productID);
            Items.Add(itemInList.ChangePrice(newPrice));
            Items.Remove(itemInList);
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
