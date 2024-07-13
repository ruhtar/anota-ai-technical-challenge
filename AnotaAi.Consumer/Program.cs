
using AnotaAi.Consumer.Options;

namespace AnotaAi.Consumer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection("RabbitMQOptions"));
        builder.Services.AddSingleton<IConsumerService, ConsumerService>();
        builder.Services.AddSingleton<ICatalogConsumerHostedService, CatalogConsumerHostedService>();
        builder.Services.AddHostedService<CatalogConsumerHostedService>();

        var app = builder.Build();

        app.Run();
    }
}
