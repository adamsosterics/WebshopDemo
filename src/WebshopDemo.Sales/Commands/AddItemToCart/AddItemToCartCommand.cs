using MediatR;
using System;

namespace WebshopDemo.Sales.Commands.AddItemToCart
{
    public class AddItemToCartCommand : IRequest
    {
        public Guid ProductID { get; set; }
        public Guid CartID { get; set; }
    }
}
