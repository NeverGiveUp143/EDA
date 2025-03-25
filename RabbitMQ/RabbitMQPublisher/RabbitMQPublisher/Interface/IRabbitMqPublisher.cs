namespace RabbitMQPublisher.Interface
{
    public interface IRabbitMqPublisher
    {
        void PublishMessage(string message, string queueName);
    }
}
