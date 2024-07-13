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

    public void PublishEvent(string ownerId)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest",
            Uri = new Uri("amqp://guest:guest@localhost:5672")  //TODO: add this to some Options class
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);
        channel.QueueDeclare(QueueName, true, false, false, null);
        channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);

        var body = Encoding.UTF8.GetBytes(ownerId);

        channel.BasicPublish(ExchangeName, RoutingKey, null, body);

        channel.Close();
        connection.Close();
    }
}
