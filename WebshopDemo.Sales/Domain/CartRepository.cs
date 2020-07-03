using System;

namespace WebshopDemo.Sales.Domain
{
    public interface CartRepository
    {
        void Add(Cart cart);
        Cart GetByID(Guid cartID);
        void Save(Cart cart);
        
    }
}
