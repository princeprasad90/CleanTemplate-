using CleanTemplate.Application.Interfaces;
using CleanTemplate.Application.Services;
using CleanTemplate.Domain.Events;
using CleanTemplate.Infrastructure.EF;
using CleanTemplate.Infrastructure.EventStore;
using Microsoft.EntityFrameworkCore;

public class OrderServiceTests
{
    private OrderService CreateService()
    {
        var options = new DbContextOptionsBuilder<EventDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new EventDbContext(options);
        IEventStore store = new EfCoreEventStore(context);
        return new OrderService(store);
    }

    [Fact]
    public async Task CreateAndAddItem_StoresEvents()
    {
        var service = CreateService();
        var id = await service.CreateAsync();
        await service.AddItemAsync(id, "Widget", 2);

        var events = await service.GetEventsAsync(id);
        Assert.Equal(2, events.Count());
        Assert.Contains(events, e => e is OrderCreated);
        Assert.Contains(events, e => e is ItemAdded);
    }
}
