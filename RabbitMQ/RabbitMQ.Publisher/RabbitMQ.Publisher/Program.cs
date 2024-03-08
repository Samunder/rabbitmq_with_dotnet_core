using EventBus.Abstractions;
using EventBus.Events;
using RabbitMQ.Publisher.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IIntegrationEventService, IntegrationEventService>();

builder.AddRabbitMqEventBus("eventbus")
         .AddSubscription<OrderStatusChangedIntegrationEvent, OrderStatusChangedIntegrationEventHandler>()
         .AddSubscription<OrderStatusChangedConfirmationIntegrationEvent, OrderStatusChangedConfirmationIntegrationEventHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();




public record OrderStatusChangedIntegrationEvent(int OrderId, string EventStatusText) : IntegrationEvent;

public class OrderStatusChangedIntegrationEventHandler(
    IIntegrationEventService integrationEventService,
    ILogger<OrderStatusChangedIntegrationEvent> logger) :IIntegrationEventHandler<OrderStatusChangedIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);


        await integrationEventService.PublishThroughEventBusAsync(new OrderStatusChangedConfirmationIntegrationEvent(@event.OrderId, $"Status has been changed for {@event.OrderId}"));
    }
}

public record OrderStatusChangedConfirmationIntegrationEvent(int OrderId, string EventStatucChangedText) : IntegrationEvent;

public class OrderStatusChangedConfirmationIntegrationEventHandler(
    ILogger<OrderStatusChangedConfirmationIntegrationEvent> logger) : IIntegrationEventHandler<OrderStatusChangedConfirmationIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedConfirmationIntegrationEvent @event)
    {
        logger.LogInformation("Handling EventStatucChanged integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);


        //await integrationEventService.PublishThroughEventBusAsync(@event);
    }
}
