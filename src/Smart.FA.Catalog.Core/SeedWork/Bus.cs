using Microsoft.Extensions.Logging;

namespace Core.SeedWork;

public class Bus : IBus
{
    private readonly ILogger<Bus> _logger;

    public Bus(ILogger<Bus> logger)
    {
        _logger = logger;
    }

    public void Send(string message)
    {
        _logger.LogInformation(message);
    }
}
