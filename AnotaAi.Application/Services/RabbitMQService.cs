using AnotaAi.Domain.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace AnotaAi.Application.Services;

public interface IRabbitMQService
{
    IModel CreateChannel();
}

public class RabbitMQService : IRabbitMQService
{
    private readonly ConnectionFactory _factory;

    public RabbitMQService(IOptions<RabbitMQOptions> rabbitMQOptions)
    {
        _factory = new ConnectionFactory
        {
            DispatchConsumersAsync = true,
            HostName = rabbitMQOptions.Value.HostName,
            VirtualHost = rabbitMQOptions.Value.VirtualHost,
            UserName = rabbitMQOptions.Value.UserName,
            Password = rabbitMQOptions.Value.Password,
            Uri = rabbitMQOptions.Value.Uri
        };
    }

    public IModel CreateChannel()
    {
        var connection = _factory.CreateConnection();
        return connection.CreateModel();
    }
}
