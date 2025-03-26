using RabbitMQ.Client;
using RabbitMQPublisher.Interface;
using System.Text;

namespace RabbitMQPublisher
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly string _hostName;
        private readonly string _username;
        private readonly string _password;

        public RabbitMqPublisher(string hostName, string username, string password)
        {
            _hostName = hostName;
            _username = username;
            _password = password;
        }

        public async Task PublishMessageAsync(string message, string exchangeName, string routingKey)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _username,
                Password = _password
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync(); 

            await channel.ExchangeDeclareAsync( 
                exchange: exchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, body: body);

            Console.WriteLine($"Message published to exchange '{exchangeName}' with routing key '{routingKey}': {message}");
        }

    }

}
