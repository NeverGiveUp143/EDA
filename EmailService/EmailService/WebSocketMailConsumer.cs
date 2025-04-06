using EmailService.Interface;
using EmailService.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMqConsumer.Interface;

namespace EmailService
{
    public class WebSocketMailConsumer : BackgroundService
    {
        private readonly IRabbitMqConsumer _rabbitMqConsumer;
        private readonly IServiceProvider _serviceProvider;
        public WebSocketMailConsumer(IRabbitMqConsumer rabbitMqConsumer, IServiceProvider serviceProvider)
        {
            _rabbitMqConsumer = rabbitMqConsumer;
            _serviceProvider = serviceProvider;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("WebSocketMailConsumer starting...");
            try
            {
                await _rabbitMqConsumer.StartConsumingAsync("order_status", Constants.OrderStatusRoutingKeys, message =>
                {
                    var eventMessage = JsonConvert.DeserializeObject<EventMessage<string>>(message);
                    var data = eventMessage?.Data;
                    var eventType = eventMessage?.EventType ?? "Unknown";

                    Task.Run(async () =>
                    {
                        if (eventMessage != null && eventMessage.Data != null && data != null)
                        {
                            var customer = JsonConvert.DeserializeObject<CustomerModel>(data);

                            if (customer != null)
                            {
                                using (var scope = _serviceProvider.CreateScope())
                                {
                                    var _notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                                    if (eventType == "order.updated")
                                    {
                                        await _notificationService.SendEmailAsync<CustomerModel>(customer,Constants.OrderPlacedSucessMailBody, Constants.OrderPlacedSucessMailSubject,Constants.OrderPlacedSucessMailMapping);
                                    }
                                    else if (eventType == "order.failed")
                                    {

                                    }
                                }
                            }
                        }
                    }).GetAwaiter().GetResult();

                    return Task.CompletedTask;
                });
                Console.WriteLine("WebSocketMailConsumer started");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in WebSocketMailConsumer: {ex.Message}");
                throw;
            }

        }

    }
}