using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AnotaAi.Consumer;

public interface IConsumerService
{
    Task ReadMessages(CancellationToken cancellationToken);
}

public class ConsumerService : IConsumerService, IDisposable
{
    private const string QueueName = "catalog-queue";
    private readonly IModel channel;
    private readonly IConnection connection;

    public ConsumerService()
    {
        var factory = new ConnectionFactory()
        {
            DispatchConsumersAsync = true,
            HostName = "localhost",
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest",
            Uri = new Uri("amqp://guest:guest@localhost:5672")  //TODO: add this to some Options class
        };

        connection = factory.CreateConnection();
        channel = connection.CreateModel();
    }

    public async Task ReadMessages(CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var ownerId = Encoding.UTF8.GetString(body);
            // copy or deserialise the payload
            // and process the message
            await Task.CompletedTask;
            channel.BasicAck(ea.DeliveryTag, false);
        };
        string consumerTag = channel.BasicConsume(QueueName, false, consumer);
        await Task.CompletedTask;
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
