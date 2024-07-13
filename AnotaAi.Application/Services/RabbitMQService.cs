using AnotaAi.Domain.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace AnotaAi.Application.Services;

public interface IRabbitMQService
{
    IModel CreateChannel();
}

public class RabbitMQService : IRabbitMQService, IDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;

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
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        return _channel;
    }
    public void Dispose()
    {
        if (_channel.IsOpen)
            _channel.Close();

        if (_connection.IsOpen)
            _connection.Close();

        GC.SuppressFinalize(this); //IDE recommends this. Learn more about this: https://learn.microsoft.com/pt-br/dotnet/fundamentals/code-analysis/quality-rules/ca1816
    }
}
