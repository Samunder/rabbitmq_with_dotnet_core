using EventBus.Abstractions;
using EventBus.Events;

namespace RabbitMQ.Publisher.IntegrationEvents
{
    public sealed class IntegrationEventService(ILogger<IntegrationEventService> logger, IEventBus eventBus): IIntegrationEventService, IDisposable
    {
        private volatile bool disposedValue;

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                logger.LogInformation("Publishing integration event: {IntegrationEventId_published} - ({@IntegrationEvent})", evt.Id, evt);
                await eventBus.PublishAsync(evt);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", evt.Id, evt);
            }
        }
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Disponse if any object need to
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
