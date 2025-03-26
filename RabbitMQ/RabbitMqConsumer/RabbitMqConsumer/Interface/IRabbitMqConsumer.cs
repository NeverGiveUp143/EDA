namespace RabbitMqConsumer.Interface
{

    public interface IRabbitMqConsumer
    {
        Task StartConsumingAsync(string queueName, Func<string, Task> messageHandler);
    }
}
