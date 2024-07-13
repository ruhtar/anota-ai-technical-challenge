using AnotaAi.Application.UseCases;
using AnotaAi.Domain.Options;
using Microsoft.Extensions.Options;
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
    private const string ExchangeName = "catalog-topic";
    private const string RoutingKey = "catalog.*";
    private readonly IModel channel;
    private readonly IConnection connection;
    private readonly ICatalogUseCase catalogUseCase;

    public ConsumerService(IOptions<RabbitMQOptions> rabbitMQOptions, ICatalogUseCase catalogUseCase)
    {
        //TODO: ABSTRACT THIS
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

        channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);
        channel.QueueDeclare(QueueName, true, false, false, null);
        channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);

        this.catalogUseCase = catalogUseCase;
    }

    public async Task ReadMessages(CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var ownerId = Encoding.UTF8.GetString(body);
            await catalogUseCase.UpdateOwnerCatalog(ownerId, cancellationToken);

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
