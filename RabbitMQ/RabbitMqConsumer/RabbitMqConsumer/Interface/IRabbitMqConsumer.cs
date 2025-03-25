namespace RabbitMqConsumer.Interface
{

    public interface IRabbitMqConsumer
    {
        void StartConsuming(string queueName, Action<string> messageHandler);
    }
}
