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

        // Start consuming from multiple queues concurrently
        public async Task StartConsumingAsync(IEnumerable<(string queueName, string exchangeName, string exchangeType, Func<string, Task> messageHandler)> consumerConfigs, CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _username,
                Password = _password
            };

            _connection = await factory.CreateConnectionAsync();

            // Start consuming messages concurrently for each queue
            var tasks = new List<Task>();
            foreach (var (queueName, exchangeName, exchangeType, messageHandler) in consumerConfigs)
            {
                var task = StartConsumingQueueAsync(queueName, exchangeName, exchangeType, messageHandler, cancellationToken);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        // Consumer logic for each queue (runs asynchronously and independently)
        private async Task StartConsumingQueueAsync(string queueName, string exchangeName, string exchangeType, Func<string, Task> messageHandler, CancellationToken cancellationToken)
        {
            var channel = await CreateConsumerChannelAsync(queueName, exchangeName, exchangeType, messageHandler, cancellationToken);
            _channels.Add(channel);

            // Keep consuming messages until cancellation is requested
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(100, cancellationToken); 
            }
        }

        // Creates a consumer channel for each queue/exchange
        private async Task<IChannel> CreateConsumerChannelAsync(string queueName, string exchangeName, string exchangeType, Func<string, Task> messageHandler, CancellationToken cancellationToken)
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
                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false); // Acknowledge the message
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true); // Requeue on failure
                }
            };

            await channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);
         
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
            Console.WriteLine("All consumers stopped.");
        }
    }
}
