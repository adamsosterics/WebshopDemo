using MediatR;
using System;

namespace WebshopDemo.Sales.Events
{
    public class PriceChanged : INotification
    {
        public Guid ProductID { get; set; }
    }
}
