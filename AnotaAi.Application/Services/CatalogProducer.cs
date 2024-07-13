using AnotaAi.Domain.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace AnotaAi.Application.Services;

public interface ICatalogProducer
{
    void PublishEvent(string ownerId);
}

public class CatalogProducer : ICatalogProducer, IDisposable
{
    private const string QueueName = "catalog-queue";
    private const string ExchangeName = "catalog-topic";
    private const string RoutingKey = "catalog.*";
    private readonly IModel channel;
    private readonly IConnection connection;

    public CatalogProducer(IOptions<RabbitMQOptions> rabbitMQOptions)
    {
        var factory = new ConnectionFactory()
        {
            DispatchConsumersAsync = true,
            HostName = rabbitMQOptions.Value.HostName,
            VirtualHost = rabbitMQOptions.Value.VirtualHost,
            UserName = rabbitMQOptions.Value.UserName,
            Password = rabbitMQOptions.Value.Password,
            Uri = rabbitMQOptions.Value.Uri
        };

        connection = factory.CreateConnection();
        channel = connection.CreateModel();
    }

    public void PublishEvent(string ownerId)
    {
        channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);
        channel.QueueDeclare(QueueName, true, false, false, null);
        channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);

        var body = Encoding.UTF8.GetBytes(ownerId);

        channel.BasicPublish(ExchangeName, RoutingKey, null, body);
    }

    public void Dispose()
    {
        if (channel.IsOpen)
            channel.Close();

        if (connection.IsOpen)
            connection.Close();

        GC.SuppressFinalize(this); //IDE recommends this. Learn more about this: https://learn.microsoft.com/pt-br/dotnet/fundamentals/code-analysis/quality-rules/ca1816
    }
}
