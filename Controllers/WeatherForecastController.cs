using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace Serilog_SubLogger_POC.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ILogger<SubLogger> _log;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger
        , ILogger<SubLogger> subLogger)
    {
        _logger = logger;
        _log = subLogger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogDebug("log sample 1");
        _logger.LogInformation("log sample 2");
        _logger.LogWarning("log sample 3");

        _logger.LogInformation("No contextual properties");
        using (LogContext.PushProperty("A", 1))
        {
            _logger.LogInformation("Carries property A = 1");

            using (LogContext.PushProperty("A", 2))
            using (LogContext.PushProperty("B", 1))
            {
                _logger.LogInformation("Carries A = 2 and B = 1");
            }

            _logger.LogInformation("Carries property A = 1, again");
        }
        _log.LogDebug("slog sample 1");
        _log.LogInformation("slog sample 2");
        _log.LogWarning("slog sample 3");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
