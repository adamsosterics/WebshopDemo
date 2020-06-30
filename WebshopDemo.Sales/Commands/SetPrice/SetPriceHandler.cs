using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Domain;

namespace WebshopDemo.Sales.Commands.SetPrice
{
    public class SetPriceHandler : IRequestHandler<SetPriceCommand>
    {
        private readonly ProductRepository productRepository;

        public SetPriceHandler(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Unit> Handle(SetPriceCommand request, CancellationToken cancellationToken)
        {
            var product = productRepository.GetByID(request.ProductID);
            product.SetPrice(new Price(request.ProductPrice.Amount, request.ProductPrice.Currency));
            productRepository.Save(product);
            return new Unit();
        }
    }
}
