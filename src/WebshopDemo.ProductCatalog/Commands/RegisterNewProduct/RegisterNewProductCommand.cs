using MediatR;
using System;

namespace WebshopDemo.ProductCatalog.Commands.RegisterNewProduct
{
    public class RegisterNewProductCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
