
using Microsoft.EntityFrameworkCore;
using EDADBContext;
using EDACustomer.Business.Interface;
using EDACustomer.Repository.Interface;
using EDACustomer.Repository;
using EDACustomer.Business;
using EDACustomer.Services.Interface;
using EDACustomer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()  // Allows requests from any frontend (React, etc.)
            .AllowAnyMethod()  // Allows GET, POST, PUT, DELETE, etc.
            .AllowAnyHeader()); // Allows all headers
});

// Add services to the container.
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(connectionString, options => options.CommandTimeout((int)TimeSpan.FromMinutes(30).TotalSeconds)), ServiceLifetime.Transient);
builder.Services.AddScoped<ICustomerBusiness, CustomerBusiness>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductBusiness, ProductBusiness>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
builder.Services.AddScoped<IConfigBusiness, ConfigBusiness>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomSignalR();


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
    _ = endpoints.MapCustomSignalR();
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
