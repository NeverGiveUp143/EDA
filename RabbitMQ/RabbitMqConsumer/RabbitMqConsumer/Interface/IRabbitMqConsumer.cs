namespace RabbitMqConsumer.Interface
{

    public interface IRabbitMqConsumer
    {
        Task StartConsumingAsync(string exchangeName, List<string> routingKeys, Func<string, Task> messageHandler);
    }
}
