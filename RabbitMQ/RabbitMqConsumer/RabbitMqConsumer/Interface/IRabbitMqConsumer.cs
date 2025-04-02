namespace RabbitMqConsumer.Interface
{

    public interface IRabbitMqConsumer
    {
        Task StartConsumingAsync(string queueName, string exchangeName, Func<string, Task> messageHandler);
    }
}
