using MediatR;
using System;

namespace WebshopDemo.Sales.Commands.RemoveItemFromCart
{
    public class RemoveItemFromCartCommand : IRequest
    {
        public Guid CartID { get; set; }
        public Guid ProductID { get; set; }
    }
}
