using System;
using System.Collections.Generic;

namespace WebshopDemo.ProductCatalog.Queries.AvailableProducts
{
    public class AvailableProducts
    {
        public AvailableProducts()
        {
            Products = new List<Product>();
        }
        public List<Product> Products { get; set; }

        public class Product
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}