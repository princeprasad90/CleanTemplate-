using CleanTemplate.Application.Interfaces;
using CleanTemplate.Application.Services;
using CleanTemplate.Infrastructure.EF;
using CleanTemplate.Infrastructure.EventStore;
using Microsoft.EntityFrameworkCore;
using CleanTemplate.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EventDbContext>(opt => opt.UseInMemoryDatabase("events"));
builder.Services.AddScoped<IEventStore, EfCoreEventStore>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

await DataSeeder.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
