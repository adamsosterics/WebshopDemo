using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebshopDemo.ProductCatalog.IntegrationTests
{
    class CollectorMediator : IMediator
    {
        private readonly Mediator mediator;
        private readonly List<INotification> notifications;

        public CollectorMediator(Mediator mediator)
        {
            this.mediator = mediator;
            notifications = new List<INotification>();
        }

        public List<INotification> Notifications => notifications;

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            notifications.Add(notification as INotification);
            return mediator.Publish(notification, cancellationToken);
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            notifications.Add(notification);
            return mediator.Publish(notification, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return mediator.Send(request, cancellationToken);
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            return mediator.Send(request, cancellationToken);
        }
    }
}
