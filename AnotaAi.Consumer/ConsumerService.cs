using AnotaAi.Application.Services;
using AnotaAi.Application.UseCases;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AnotaAi.Consumer;

public interface IConsumerService
{
    Task ReadMessages(CancellationToken cancellationToken);
}

public class ConsumerService : IConsumerService
{
    private const string QueueName = "catalog-queue";
    private const string ExchangeName = "catalog-topic";
    private const string RoutingKey = "catalog.*";
    private readonly IModel channel;
    private readonly ICatalogUseCase catalogUseCase;

    public ConsumerService(ICatalogUseCase catalogUseCase, IRabbitMQService rabbitMQService)
    {
        channel = rabbitMQService.CreateChannel();

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
            var _ = await catalogUseCase.UpdateOwnerCatalog(ownerId, cancellationToken);
            await Task.CompletedTask;
            channel.BasicAck(ea.DeliveryTag, false);
        };
        string consumerTag = channel.BasicConsume(QueueName, false, consumer);
        await Task.CompletedTask;
    }
}
