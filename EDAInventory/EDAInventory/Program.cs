using EDADBContext;
using EDAInventory.Business;
using EDAInventory.Business.Interface;
using EDAInventory.Repository;
using EDAInventory.Repository.Interface;
using EDAInventory.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMqConsumer;
using RabbitMqConsumer.Interface;
using RabbitMQPublisher.Interface;
using RabbitMQPublisher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()  // Allows requests from any frontend (React, etc.)
            .AllowAnyMethod()  // Allows GET, POST, PUT, DELETE, etc.
            .AllowAnyHeader()); // Allows all headers
});

builder.Services.AddSingleton<IRabbitMqConsumer>(sp =>
    new RabbitMQConsumer(
        builder.Configuration["RabbitMQ:HostName"] ?? "localhost",
        builder.Configuration["RabbitMQ:Username"] ?? "guest",
        builder.Configuration["RabbitMQ:Password"] ?? "guest"
    ));

builder.Services.AddSingleton<IRabbitMqPublisher>(sp =>
    new RabbitMqPublisher(
        hostName: builder.Configuration["RabbitMQ:HostName"] ?? "localhost",
        username: builder.Configuration["RabbitMQ:Username"] ?? "guest",
        password: builder.Configuration["RabbitMQ:Password"] ?? "guest"
    ));

builder.Services.AddHostedService<WebSocketOrderConsumer>();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(connectionString, options => options.CommandTimeout((int)TimeSpan.FromMinutes(30).TotalSeconds)), ServiceLifetime.Transient);
builder.Services.AddScoped<IProductBusiness, ProductBusiness>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
builder.Services.AddScoped<IConfigBusiness, ConfigBusiness>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); // Moved before UseEndpoints
app.UseCors("AllowAllOrigins");
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers(); // Maps controller routes
    _ = endpoints.MapGet("/ws", async context =>
    {
        var wsService = app.Services.GetRequiredService<WebSocketOrderConsumer>();
        
    });
});

// No need for app.MapControllers() here; it's already in UseEndpoints

app.UseHttpsRedirection();
app.Run();