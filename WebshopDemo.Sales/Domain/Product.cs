using System;

namespace WebshopDemo.Sales.Domain
{
    public class Product
    {
        public Product(Guid id)
        {
            Id = id;
        }

        private Product() { }

        public Guid Id { get; private set; }
        public Price Price { get; internal set; }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Id.Equals(product.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public void SetPrice(Price price)
        {
            Price = price;
        }
    }
}
