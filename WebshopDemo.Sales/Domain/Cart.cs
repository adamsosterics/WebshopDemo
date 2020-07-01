using System;

namespace WebshopDemo.Sales.Domain
{
    public class Cart
    {
        public Cart(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
        public CartState State { get; set; }
    }
}
