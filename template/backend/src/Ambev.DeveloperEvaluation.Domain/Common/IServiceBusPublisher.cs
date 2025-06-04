namespace Ambev.DeveloperEvaluation.Application.Common;

public interface IServiceBusPublisher
{
    Task PublishAsync(string topic, object message, CancellationToken cancellationToken = default);
}