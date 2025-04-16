namespace RabbitMqConsumer.Interface
{

    public interface IRabbitMqConsumer
    {
        Task StartConsumingAsync(IEnumerable<(string queueName, string exchangeName, string exchangeType, Func<string, Task> messageHandler)> consumerConfigs, CancellationToken cancellationToken);
    }
}
