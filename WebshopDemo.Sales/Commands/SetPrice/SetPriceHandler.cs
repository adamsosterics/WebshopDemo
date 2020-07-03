using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebshopDemo.Sales.Domain;
using WebshopDemo.Sales.Events;

namespace WebshopDemo.Sales.Commands.SetPrice
{
    public class SetPriceHandler : IRequestHandler<SetPriceCommand>
    {
        private readonly ProductRepository productRepository;
        private readonly IMediator mediator;

        public SetPriceHandler(ProductRepository productRepository, IMediator mediator)
        {
            this.productRepository = productRepository;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(SetPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = productRepository.GetByID(request.ProductID);
                product.SetPrice(new Price(request.ProductPrice.Amount, request.ProductPrice.Currency));
                productRepository.Save(product);

                await mediator.Send(new PriceChanged { ProductID = request.ProductID });

                return new Unit();
            }
            catch (ProductNotFoundException ex)
            {
                var exc = new SetPriceException("Product not found", ex);
                exc.Data.Add("ProductID", request.ProductID);
                throw exc;
            }
        }
    }
}
