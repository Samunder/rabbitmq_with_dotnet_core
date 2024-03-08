using EventBus.Events;

namespace RabbitMQ.Publisher.IntegrationEvents
{
    public interface IIntegrationEventService
    {
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
