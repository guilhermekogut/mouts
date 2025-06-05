
using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Events;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events
{
    public class SaleUpdatedEventHandler : INotificationHandler<SaleUpdatedEvent>
    {
        private readonly IServiceBusPublisher _serviceBusPublisher;
        private readonly ILogger<SaleCreatedEventHandler> _logger;

        public SaleUpdatedEventHandler(IServiceBusPublisher serviceBusPublisher, ILogger<SaleCreatedEventHandler> logger)
        {
            _serviceBusPublisher = serviceBusPublisher;
            _logger = logger;
        }

        public async Task Handle(SaleUpdatedEvent notification, CancellationToken cancellationToken)
        {
            // Serialize and send to ServiceBus
            await _serviceBusPublisher.PublishAsync("sales", notification, cancellationToken);
            _logger.LogInformation("SaleUpdatedEvent published to ServiceBus: {SaleId}", notification.Sale.Id);
        }
    }
}