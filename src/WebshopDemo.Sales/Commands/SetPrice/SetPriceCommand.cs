using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebshopDemo.Sales.Commands.SetPrice
{
    public class SetPriceCommand : IRequest
    {
        public Guid ProductID { get; set; }
        public Price ProductPrice { get; set; }

        public class Price
        {
            public string Currency { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
