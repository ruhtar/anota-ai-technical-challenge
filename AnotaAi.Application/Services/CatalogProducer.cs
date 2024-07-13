using RabbitMQ.Client;
using System.Text;

namespace AnotaAi.Application.Services;

public interface ICatalogProducer
{
    void PublishEvent(string ownerId);
}

public class CatalogProducer : ICatalogProducer
{
    private const string QueueName = "catalog-queue";
    private const string ExchangeName = "catalog-topic";
    private const string RoutingKey = "catalog.*";
    private readonly IModel channel;

    public CatalogProducer(IRabbitMQService rabbitMQService)
    {
        channel = rabbitMQService.CreateChannel();

        channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);
        channel.QueueDeclare(QueueName, true, false, false, null);
        channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);
    }

    public void PublishEvent(string ownerId)
    {
        var body = Encoding.UTF8.GetBytes(ownerId);

        channel.BasicPublish(ExchangeName, RoutingKey, null, body);
    }
}
