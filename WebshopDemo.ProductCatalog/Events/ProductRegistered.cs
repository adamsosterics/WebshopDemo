using MediatR;
using System;

namespace WebshopDemo.ProductCatalog.Events
{
    public class ProductRegistered : INotification
    {
        public Guid ProductID { get; set; }
    }
}
