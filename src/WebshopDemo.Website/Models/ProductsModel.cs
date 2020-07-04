using System;
using System.Collections.Generic;

namespace WebshopDemo.Website.Models
{
    public class ProductsModel
    {
        public List<Product> Products { get; set; }

        public class Product
        {
            public Guid ProductID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public Price Price { get; set; }

        }
        public class Price
        {
            public decimal Amount { get; set; }
            public string Currency { get; set; }
        }
    }
}
