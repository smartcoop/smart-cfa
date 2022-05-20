using Microsoft.Extensions.Logging;

namespace Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

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
