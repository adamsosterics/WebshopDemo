using System;

namespace WebshopDemo.Sales.Domain
{
    public class Product
    {
        public Product(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Id.Equals(product.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
