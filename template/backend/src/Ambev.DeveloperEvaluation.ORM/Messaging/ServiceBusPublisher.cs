using Ambev.DeveloperEvaluation.Application.Common;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Messaging
{
    public class ServiceBusPublisher : IServiceBusPublisher
    {
        private readonly ILogger<ServiceBusPublisher> _logger;

        public ServiceBusPublisher(ILogger<ServiceBusPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync(string topic, object message, CancellationToken cancellationToken = default)
        {
            // Todo logic to external services (ex: Azure Service Bus, RabbitMQ, etc)
            _logger.LogInformation("Publishing message to topic '{Topic}': {Message}", topic, message);
            return Task.CompletedTask;
        }
    }
}