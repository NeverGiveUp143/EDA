using EDAInventory.Business.Interface;
using EDAInventory.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMqConsumer.Interface;
using RabbitMQPublisher.Interface;
using System.Net.WebSockets;
using System.Text;

namespace EDAInventory.Services
{
    public class WebSocketOrderConsumer : BackgroundService
    {
        private readonly IRabbitMqConsumer _rabbitMqConsumer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;
        private readonly List<WebSocket> _clients = new();
        public WebSocketOrderConsumer(IRabbitMqConsumer rabbitMqConsumer, IServiceProvider serviceProvider, IRabbitMqPublisher rabbitMqPublisher)
        {
            _rabbitMqConsumer = rabbitMqConsumer;
            _serviceProvider = serviceProvider;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("WebSocketOrderConsumer starting...");
            try
            {
                var consumerConfigs = new List<(string queueName, string exchangeName, string exchangeType, Func<string, Task> messageHandler)>
        {
            (
                "inventory_queue",
                "payment_exchange",
                ExchangeType.Fanout,
                async message =>
                {
                    var eventMessage = JsonConvert.DeserializeObject<EventMessage<string>>(message);
                    var data = eventMessage?.Data;
                    var eventType = eventMessage?.EventType ?? "Unknown";

                    if (eventMessage != null && !string.IsNullOrEmpty(data))
                    {
                        var customer = JsonConvert.DeserializeObject<CustomerModel>(data);

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            if (eventType == "payment.sucess")
                            {
                                var _productBusiness = scope.ServiceProvider.GetRequiredService<IProductBusiness>();
                                Guid.TryParse(customer?.Product, out Guid productId);
                                int itemInCart = customer?.ItemInCart ?? 0;

                                string result = await _productBusiness.DeductStock(productId, itemInCart);
                                var outgoingMessage = string.Empty;

                                if (string.IsNullOrEmpty(result))
                                {
                                    outgoingMessage = JsonConvert.SerializeObject(new
                                    {
                                        eventType = "order.updated",
                                        data = JsonConvert.SerializeObject(customer)
                                    });
                                    await _rabbitMqPublisher.PublishMessageAsync(outgoingMessage, "order_exchange", "order.updated", "order_queue", ExchangeType.Topic);
                                }
                                else
                                {
                                    outgoingMessage = JsonConvert.SerializeObject(new
                                    {
                                        eventType = "order.failed",
                                        data = JsonConvert.SerializeObject(customer)
                                    });
                                    await _rabbitMqPublisher.PublishMessageAsync(outgoingMessage, "order_exchange", "order.failed", "order_queue", ExchangeType.Topic);
                                }

                                Console.WriteLine($"Consumed message: {outgoingMessage}");

                            }
                        }
                    }
                }
            )
        };

                await _rabbitMqConsumer.StartConsumingAsync(consumerConfigs);

                Console.WriteLine("WebSocketOrderConsumer started");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in WebSocketOrderConsumer: {ex.Message}");
                throw;
            }
        }


    }
}