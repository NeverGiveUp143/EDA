namespace RabbitMQPublisher.Interface
{
    public interface IRabbitMqPublisher
    {
        Task PublishMessageAsync(string message, string exchangeName, string routingKey, string queueName, string exchageType);
    }
}
