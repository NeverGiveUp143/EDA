
using Microsoft.EntityFrameworkCore;
using EDADBContext;
using OrderService.Business.Interface;
using OrderService.Repository.Interface;
using OrderService.Repository;
using OrderService.Business;
using RabbitMQPublisher.Interface;
using RabbitMQPublisher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()  
            .AllowAnyMethod()  
            .AllowAnyHeader()); 
});

// Register the publisher with configuration
builder.Services.AddSingleton<IRabbitMqPublisher>(sp =>
    new RabbitMqPublisher(
        hostName: builder.Configuration["RabbitMQ:HostName"] ?? "localhost",
        username: builder.Configuration["RabbitMQ:Username"] ?? "guest",
        password: builder.Configuration["RabbitMQ:Password"] ?? "guest"
    ));

// Add services to the container.
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(connectionString, options => options.CommandTimeout((int)TimeSpan.FromMinutes(30).TotalSeconds)), ServiceLifetime.Transient);
builder.Services.AddScoped<IOrderBusiness, OrderBusiness>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductBusiness, ProductBusiness>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
builder.Services.AddScoped<IConfigBusiness, ConfigBusiness>();
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
