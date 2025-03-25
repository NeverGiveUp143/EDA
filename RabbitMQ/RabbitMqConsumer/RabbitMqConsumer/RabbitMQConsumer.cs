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
        private IModel _channel;

        public RabbitMQConsumer(string hostName, string username, string password)
        {
            _hostName = hostName;
            _username = username;
            _password = password;
        }

        public void StartConsuming(string queueName, Action<string> messageHandler)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                messageHandler(message); // Call the provided handler
            };

            _channel.BasicConsume(queue: queueName,
                                 autoAck: true, // Set to false for manual acknowledgment if needed
                                 consumer: consumer);
        }

        // Cleanup (optional, if used outside a hosted service)
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
