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
        private List<IChannel> _channels;

        public RabbitMQConsumer(string hostName, string username, string password)
        {
            _hostName = hostName;
            _username = username;
            _password = password;
            _channels = new List<IChannel>();
        }

        // Start consuming from multiple queues (or a single one)
        public async Task StartConsumingAsync(IEnumerable<(string queueName, string exchangeName, string exchangeType, Func<string, Task> messageHandler)> consumerConfigs)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _username,
                Password = _password
            };

            _connection = await factory.CreateConnectionAsync();

            foreach (var (queueName, exchangeName, exchangeType, messageHandler) in consumerConfigs)
            {
                var channel = await CreateConsumerChannelAsync(queueName, exchangeName, exchangeType, messageHandler);
                _channels.Add(channel);
            }

            Console.WriteLine("All consumers started.");
        }

        // Creates a consumer channel for each queue/exchange
        private async Task<IChannel> CreateConsumerChannelAsync(string queueName, string exchangeName, string exchangeType, Func<string, Task> messageHandler)
        {
            var channel = await _connection.CreateChannelAsync();

            // Declare the exchange and queue
            await channel.ExchangeDeclareAsync(exchangeName, exchangeType, durable: true, autoDelete: false);
            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);

            var bindingRoutingKey = exchangeType == ExchangeType.Fanout ? string.Empty : queueName;
            await channel.QueueBindAsync(queueName, exchangeName, bindingRoutingKey);

            // Set up the consumer
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    await messageHandler(message);
                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);
            Console.WriteLine($"Consumer started for '{queueName}' bound to '{exchangeName}' ({exchangeType})");

            return channel;
        }

        // Gracefully stop consumers and close channels
        public async Task StopConsumingAsync()
        {
            foreach (var channel in _channels)
            {
                // Close and dispose each channel
                await channel.CloseAsync();
                channel.Dispose();
            }

            _connection?.CloseAsync();
            _connection?.Dispose();
            Console.WriteLine(" All consumers stopped.");
        }
    }
}
