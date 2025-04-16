using EDAInventory.Business.Interface;
using EDAInventory.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMqConsumer.Interface;
using RabbitMQPublisher.Interface;

namespace EDAInventory.Services
{
    public class OrderCheckOutConsumer : BackgroundService
    {
        private readonly IRabbitMqConsumer _rabbitMqConsumer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;
        public OrderCheckOutConsumer(IRabbitMqConsumer rabbitMqConsumer, IServiceProvider serviceProvider, IRabbitMqPublisher rabbitMqPublisher)
        {
            _rabbitMqConsumer = rabbitMqConsumer;
            _serviceProvider = serviceProvider;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var consumerConfigs = new List<(string queueName, string exchangeName, string exchangeType, Func<string, Task> messageHandler)>
                {
                    ("inventory_queue",
                        "payment_exchange",
                        ExchangeType.Fanout,
                        async message =>
                        {
                            var eventMessage = JsonConvert.DeserializeObject<EventMessage<string>>(message);
                            var data = eventMessage?.Data;
                            var eventType = eventMessage?.EventType ?? "Unknown";

                            if (eventMessage != null && !string.IsNullOrEmpty(data))
                            {
                                var order = JsonConvert.DeserializeObject<OrderModel>(data);

                                using (var scope = _serviceProvider.CreateScope())
                                {
                                    if (eventType == "payment.sucess")
                                    {
                                        var _inventoryBusiness = scope.ServiceProvider.GetRequiredService<IInventoryBusiness>();
                                        Guid.TryParse(order?.Product, out Guid productId);
                                        int itemInCart = order?.ItemInCart ?? 0;

                                        string result = await _inventoryBusiness.ModifyStock(productId, itemInCart);
                                        var outgoingMessage = string.Empty;

                                        if (string.IsNullOrEmpty(result))
                                        {
                                            outgoingMessage = JsonConvert.SerializeObject(new
                                            {
                                                eventType = "order.updated",
                                                data = JsonConvert.SerializeObject(order)
                                            });
                                            await _rabbitMqPublisher.PublishMessageAsync(outgoingMessage, "order_exchange", "order.updated", "order_queue", ExchangeType.Topic);
                                        }
                                        else
                                        {
                                            outgoingMessage = JsonConvert.SerializeObject(new
                                            {
                                                eventType = "order.failed",
                                                data = JsonConvert.SerializeObject(order)
                                            });
                                            await _rabbitMqPublisher.PublishMessageAsync(outgoingMessage, "order_exchange", "order.failed", "order_queue", ExchangeType.Topic);
                                        }
                                    }
                                }
                            }
                        })
                };

                await _rabbitMqConsumer.StartConsumingAsync(consumerConfigs, stoppingToken);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in WebSocketOrderConsumer: {ex.Message}");
                throw;
            }
        }


    }
}