using EDAInventory.Business.Interface;
using EDAInventory.Models;
using Newtonsoft.Json;
using RabbitMqConsumer.Interface;
using System.Net.WebSockets;
using System.Text;

namespace EDAInventory.Services
{
    public class WebSocketOrderConsumer : BackgroundService
    {
        private readonly IRabbitMqConsumer _rabbitMqConsumer;
        private readonly IServiceProvider _serviceProvider; // Replace IProductBusiness with IServiceProvider
        private readonly List<WebSocket> _clients = new();
        public WebSocketOrderConsumer(IRabbitMqConsumer rabbitMqConsumer, IServiceProvider serviceProvider)
        {
            _rabbitMqConsumer = rabbitMqConsumer;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("WebSocketOrderConsumer starting...");
            try
            {
                await _rabbitMqConsumer.StartConsumingAsync("order_exchange", message =>
                {
                    Console.WriteLine($"Consumed message: {message}");
                    var eventMessage = JsonConvert.SerializeObject(new { eventType = "OrderPlaced", data = JsonConvert.DeserializeObject(message) });
                    Task.Run(async () =>
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var productBusiness = scope.ServiceProvider.GetRequiredService<IProductBusiness>();
                            var product = JsonConvert.DeserializeObject<ProductModel>(message);
                            await productBusiness.UpsertProduct(product, true);
                        }

                        lock (_clients)
                        {
                            Console.WriteLine($"Broadcasting to {_clients.Count} clients");
                            foreach (var client in _clients.ToArray())
                            {
                                if (client.State == WebSocketState.Open)
                                {
                                    var buffer = Encoding.UTF8.GetBytes(eventMessage);
                                    client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, stoppingToken);
                                    Console.WriteLine("Message sent to client");
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