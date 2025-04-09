namespace RabbitMqConsumer.Interface
{

    public interface IRabbitMqConsumer
    {
        Task StartConsumingAsync(string queueName, string exchangeName, string exchangeType, Func<string, Task> messageHandler);
    }
}
