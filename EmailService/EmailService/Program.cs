using EDADBContext;
using EmailService;
using EmailService.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMqConsumer;
using RabbitMqConsumer.Interface;
class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                IConfiguration configuration = context.Configuration;
                string? connectionString = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<DBContext>(options => options.UseSqlServer(connectionString, options => 
                options.CommandTimeout((int)TimeSpan.FromMinutes(30).TotalSeconds)), ServiceLifetime.Transient);

                services.AddScoped<INotificationService, NotificationService>();
                services.AddScoped<IConfigBusiness, ConfigBusiness>();
                services.AddScoped<IConfigRepository, ConfigRepository>();
                services.AddSingleton<IRabbitMqConsumer>(sp =>
                        new RabbitMQConsumer(
                            configuration["RabbitMQ:HostName"] ?? "localhost",
                            configuration["RabbitMQ:Username"] ?? "guest",
                            configuration["RabbitMQ:Password"] ?? "guest"
                        ));

                services.AddHostedService<NotificationConsumer>();
            })
            .Build();

        await host.RunAsync();
    }
}
