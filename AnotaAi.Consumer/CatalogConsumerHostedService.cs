
namespace AnotaAi.Consumer;

public interface ICatalogConsumerHostedService
{
    Task StartAsync(CancellationToken cancellationToken);
    Task StopAsync(CancellationToken cancellationToken);
}

public class CatalogConsumerHostedService : IHostedService, ICatalogConsumerHostedService
{
    private readonly IConsumerService _consumerService;

    public CatalogConsumerHostedService(IConsumerService consumerService)
    {
        _consumerService = consumerService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _consumerService.ReadMessages(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
