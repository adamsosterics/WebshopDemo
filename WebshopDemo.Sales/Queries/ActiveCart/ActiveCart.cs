using System;

namespace WebshopDemo.Sales.Queries.ActiveCart
{
    public class ActiveCart
    {
        public Guid CartID { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ActiveCart cart &&
                   CartID.Equals(cart.CartID);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CartID);
        }
    }
}
