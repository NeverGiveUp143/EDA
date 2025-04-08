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
            try
            {
                await _rabbitMqConsumer.StartConsumingAsync("order_status", Constants.OrderStatusRoutingKeys, HandleMessageAsync);

                
                await _rabbitMqConsumer.StartConsumingAsync("payment_status", Constants.PaymentStatusRoutingKeys, HandleMessageAsync);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in WebSocketMailConsumer: {ex.Message}");
                throw;
            }
        }

        private Task HandleMessageAsync(string message)
        {
            var eventMessage = JsonConvert.DeserializeObject<EventMessage<string>>(message);
            var data = eventMessage?.Data;
            var eventType = eventMessage?.EventType ?? "Unknown";

            Task.Run(async () =>
            {
                if (eventMessage != null && !string.IsNullOrEmpty(data))
                {
                    var customer = JsonConvert.DeserializeObject<CustomerModel>(data);

                    if (customer != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var _notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                        switch (eventType)
                        {
                            case "order.updated":
                                await _notificationService.SendEmailAsync(customer,
                                    Constants.OrderPlacedSucessMailBody,
                                    Constants.OrderPlacedSucessMailSubject,
                                    Constants.OrderPlacedSucessMailMapping);
                                break;                        
                            case "payment.sucess":
                                await _notificationService.SendEmailAsync(customer,
                                    Constants.PaymentSucessMailBody,
                                    Constants.PaymentSucessMailSubject,
                                    Constants.PaymentSucessMailMapping);
                                break;
                            case "payment.failed":
                                await _notificationService.SendEmailAsync(customer,
                                    Constants.PaymentFailedMailBody,
                                    Constants.PaymentFailedMailSubject,
                                    Constants.PaymentFailedMailMapping);
                                break;

                            default:
                                Console.WriteLine($"Unhandled event type: {eventType}");
                                break;
                        }
                    }
                }
            }).GetAwaiter().GetResult();

            return Task.CompletedTask;
        }


    }
}