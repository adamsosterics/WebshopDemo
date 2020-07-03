using System;
using System.Collections.Generic;

namespace WebshopDemo.Sales.Domain
{
    public interface CartRepository
    {
        void Add(Cart cart);
        Cart GetByID(Guid cartID);
        List<Cart> GetCartsContainingProduct(Guid productID);
        void Save(Cart cart);
        
    }
}
