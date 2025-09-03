using Xunit;
using CleanTemplate.Application.Interfaces;
using CleanTemplate.Domain.Events;
using CleanTemplate.Infrastructure.EF;
using CleanTemplate.Infrastructure.EventStore;
using Microsoft.EntityFrameworkCore;

public class EventStoreSnapshotTests
{
    private EfCoreEventStore CreateStore()
    {
        var options = new DbContextOptionsBuilder<EventDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new EventDbContext(options);
        return new EfCoreEventStore(context);
    }

    private record ProductSnapshot(string Name);

    [Fact]
    public async Task SaveAndLoadSnapshot_ReturnsRemainingEvents()
    {
        var store = CreateStore();
        var id = Guid.NewGuid();
        await store.SaveAsync(id, new IEvent[] { new ProductCreated(id, "Test", DateTime.UtcNow) });
        await store.SaveSnapshotAsync(id, new ProductSnapshot("Test"));
        await store.SaveAsync(id, new IEvent[] { new ProductSubmitted(id, DateTime.UtcNow) });

        var (snapshot, events) = await store.GetWithSnapshotAsync<ProductSnapshot>(id);

        Assert.NotNull(snapshot);
        Assert.Equal("Test", snapshot!.Name);
        Assert.Single(events);
        Assert.Contains(events, e => e is ProductSubmitted);
    }
}
