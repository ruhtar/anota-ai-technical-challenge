using AnotaAi.Application.Services;
using AnotaAi.Application.UseCases;
using AnotaAi.Domain.Options;
using AnotaAi.Infraestructure.Repositories;

namespace AnotaAi.Consumer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection("RabbitMQOptions"));

        builder.Services.AddSingleton<IProductRepository, ProductRepository>();
        builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();

        builder.Services.AddSingleton<ICategoryService, CategoryService>();
        builder.Services.AddSingleton<ICatalogService, CatalogService>();
        builder.Services.AddSingleton<IProductService, ProductService>();
        builder.Services.AddSingleton<ICatalogUseCase, CatalogUseCase>();
        builder.Services.AddSingleton<IConsumerService, ConsumerService>();
        builder.Services.AddSingleton<ICatalogConsumerHostedService, CatalogConsumerHostedService>();
        builder.Services.AddHostedService<CatalogConsumerHostedService>();

        var app = builder.Build();

        app.Run();
    }
}
