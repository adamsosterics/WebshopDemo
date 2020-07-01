using System;

namespace WebshopDemo.Sales.Domain
{
    public interface CartRepository
    {
        Cart GetByID(Guid cartID);
        void Save(Cart cart);
        
    }
}
