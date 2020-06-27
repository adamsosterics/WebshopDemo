using MediatR;
using System;

namespace WebshopDemo.ProductCatalog.Events
{
    class ProductRegistered : INotification
    {
        public Guid ProductID { get; set; }
    }
}
