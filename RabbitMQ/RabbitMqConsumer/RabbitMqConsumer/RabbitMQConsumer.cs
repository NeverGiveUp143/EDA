using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqConsumer.Interface;
using System.Text;
using System.Threading.Channels;

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

        public async Task StartConsumingAsync(string queueName, string exchangeName, string exchangeType, Func<string, Task> messageHandler)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _username,
                Password = _password
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            // Declare the exchange
            await _channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: exchangeType,
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

            // ❗️Bind the queue to the exchange
            var bindingRoutingKey = exchangeType == ExchangeType.Fanout ? string.Empty : queueName;
            await _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: bindingRoutingKey);

            // Set up consumer
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await messageHandler(message);
                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

            Console.WriteLine($"✅ Started consuming from '{queueName}' bound to '{exchangeName}' ({exchangeType})");
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
