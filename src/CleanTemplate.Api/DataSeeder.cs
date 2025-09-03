using CleanTemplate.Application.Services;
using CleanTemplate.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.Api;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EventDbContext>();
        if (await context.Events.AnyAsync())
        {
            return;
        }

        var orders = scope.ServiceProvider.GetRequiredService<OrderService>();
        var id = await orders.CreateAsync();
        await orders.AddItemAsync(id, "Sample", 1);
    }
}
