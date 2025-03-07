using EDADBContext;
using EDAInventory.Business;
using EDAInventory.Business.Interface;
using EDAInventory.Repository;
using EDAInventory.Repository.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(connectionString, options => options.CommandTimeout((int)TimeSpan.FromMinutes(30).TotalSeconds)), ServiceLifetime.Transient);
builder.Services.AddScoped<IProductBusiness, ProductBusiness>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
builder.Services.AddScoped<IConfigBusiness, ConfigBusiness>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
