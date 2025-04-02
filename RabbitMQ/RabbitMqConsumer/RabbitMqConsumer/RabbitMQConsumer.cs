using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqConsumer.Interface;
using System.Text;

namespace RabbitMqConsumer
{
    public class RabbitMQConsumer : IRabbitMqConsumer
    {
        private readonly string _hostName;
        private readonly string _username;
        private readonly string _password;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMQConsumer(string hostName, string username, string password)
        {
            _hostName = hostName;
            _username = username;
            _password = password;
        }

        public async Task StartConsumingAsync(string queueName, string exchangeName, Func<string, Task> messageHandler)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _username,
                Password = _password
            };

            // Create connection and channel asynchronously
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            // Declare the topic exchange
            await _channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null);

            // Declare the queue
            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Bind the queue to the exchange with the routing key
            await _channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: "order.sucess");

            await _channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: "order.failed");

            // Set up the consumer
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");

                // Process the message using the provided handler
                await messageHandler(message);

                // Acknowledge the message
                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            // Start consuming
            await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        }

        public async Task DisposeAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }
            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }
        }
    }
}
