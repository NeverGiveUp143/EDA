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
                await _rabbitMqConsumer.StartConsumingAsync("inventory_queue", "payment_exchange", ExchangeType.Fanout, message =>
                {
                    var eventMessage = JsonConvert.DeserializeObject<EventMessage<string>>(message);
                    var data = eventMessage?.Data;
                    var eventType = eventMessage?.EventType ?? "Unknown";

                    Task.Run(async () =>
                    {
                        if (eventMessage != null && eventMessage.Data != null && data != null)
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
                                    var message = string.Empty;
                                    if (string.IsNullOrEmpty(result))
                                    {
                                        message = JsonConvert.SerializeObject(new { eventType = "order.updated", data = JsonConvert.SerializeObject(customer) });
                                        await _rabbitMqPublisher.PublishMessageAsync(message, "order_exchange", "order.updated", "order_queue", ExchangeType.Topic);
                                    }
                                    else
                                    {
                                        message = JsonConvert.SerializeObject(new { eventType = "order.failed", data = JsonConvert.SerializeObject(customer) });
                                        await _rabbitMqPublisher.PublishMessageAsync(message, "order_exchange", "order.failed", "order_queue", ExchangeType.Topic);
                                    }
                                    Console.WriteLine($"Consumed message: {message}");
                                }
                            }

                            lock (_clients)
                            {
                                Console.WriteLine($"Broadcasting to {_clients.Count} clients");
                                foreach (var client in _clients.ToArray())
                                {
                                    if (client.State == WebSocketState.Open)
                                    {
                                        var buffer = Encoding.UTF8.GetBytes(message);
                                        client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, stoppingToken);
                                        Console.WriteLine("Message sent to client");
                                    }
                                }
                            }
                        }
                    }).GetAwaiter().GetResult();

                    return Task.CompletedTask;
                });
                Console.WriteLine("WebSocketOrderConsumer started");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in WebSocketOrderConsumer: {ex.Message}");
                throw;
            }

        }

        public async Task HandleWebSocket(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                lock (_clients)
                {
                    _clients.Add(webSocket);
                    Console.WriteLine($"WebSocket client connected. Total clients: {_clients.Count}");
                }

                var buffer = new byte[1024 * 4];
                while (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                lock (_clients)
                {
                    _clients.Remove(webSocket);
                    Console.WriteLine($"WebSocket client disconnected. Total clients: {_clients.Count}");
                }
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }
    }
}