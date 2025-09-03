using Xunit;
using CleanTemplate.Application.Interfaces;
using CleanTemplate.Application.Services;
using CleanTemplate.Domain.Events;
using CleanTemplate.Infrastructure.EF;
using CleanTemplate.Infrastructure.EventStore;
using Microsoft.EntityFrameworkCore;

public class ProductServiceTests
{
    private ProductService CreateService()
    {
        var options = new DbContextOptionsBuilder<EventDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new EventDbContext(options);
        IEventStore store = new EfCoreEventStore(context);
        return new ProductService(store);
    }

    [Fact]
    public async Task CreateSubmitReject_StoresEvents()
    {
        var service = CreateService();
        var id = await service.CreateAsync("Test");
        await service.SubmitAsync(id);
        await service.RejectAsync(id, "not good");

        var events = await service.GetEventsAsync(id);
        Assert.Equal(3, events.Count());
        Assert.Contains(events, e => e is ProductCreated);
        Assert.Contains(events, e => e is ProductSubmitted);
        Assert.Contains(events, e => e is ProductRejected);
    }

    [Fact]
    public async Task RejectWithoutComment_Throws()
    {
        var service = CreateService();
        var id = await service.CreateAsync("Test");
        await service.SubmitAsync(id);

        await Assert.ThrowsAsync<ArgumentException>(() => service.RejectAsync(id, string.Empty));
    }
}
