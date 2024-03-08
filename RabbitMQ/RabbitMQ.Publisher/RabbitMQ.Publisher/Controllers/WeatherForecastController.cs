using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Publisher.IntegrationEvents;

namespace RabbitMQ.Publisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IIntegrationEventService _integrationEventService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IIntegrationEventService integrationEventService)
        {
            _logger = logger;
            _integrationEventService = integrationEventService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            await _integrationEventService.PublishThroughEventBusAsync(new OrderStatusChangedIntegrationEvent(1, $"Success{1}"));
            return Enumerable.Range(1, 5).Select(index =>
            {
                return new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                };
            }).ToArray();
        }
    }
}
