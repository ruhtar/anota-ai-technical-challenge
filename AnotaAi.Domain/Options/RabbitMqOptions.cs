namespace AnotaAi.Domain.Options;

public class RabbitMQOptions
{
    public required string HostName { get; set; }
    public required string VirtualHost { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public Uri Uri => new($"amqp://{UserName}:{Password}@{HostName}:5672");
}
