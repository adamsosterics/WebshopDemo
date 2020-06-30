using System;

namespace WebshopDemo.Sales.Domain
{
    public interface ProductRepository
    {
        void Add(Product product);
        Product GetByID(Guid id);
        void Save(Product product);
    }
}
