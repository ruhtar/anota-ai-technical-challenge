using RabbitMQ.Client;
using System.Text;

namespace AnotaAi.Application.Services;

public interface ICatalogService
{
    void PublishEvent(IEnumerable<string> ownerIds);
}

public class CatalogService : ICatalogService
{
    private const string QueueName = "catalog-queue";
    private const string ExchangeName = "catalog-topic";
    private const string RoutingKey = "catalog.*";

    //todos
    //API: APÓS A ATUALIZAÇÃO DE UM PRODUTO/CATEGORIA, DISPARAR UM EVENTO QUE INFORMA QUAL OWNER ID TEVE ALGUMA ATUALIZAÇÃO
    public void PublishEvent(IEnumerable<string> ownerIds)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest",
            Uri = new Uri("http://localhost:15672")  //TODO: add this to some Options class
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);
        channel.QueueDeclare(QueueName, true, false, false, null);
        channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);

        var body = ownerIds.SelectMany(s => Encoding.UTF8.GetBytes(s)).ToArray();

        channel.BasicPublish(ExchangeName, RoutingKey, null, body);

        channel.Close();
        connection.Close();
    }
    //CONSUMER: RECEBE ESSE OWNER ID -> CONSULTA A BASE -> SALVA NO JSON A INFO DO OWNER
}
