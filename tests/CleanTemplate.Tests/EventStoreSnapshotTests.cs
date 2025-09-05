using Xunit;
using CleanTemplate.Application.Interfaces;
using CleanTemplate.Domain.Events;
using CleanTemplate.Domain.Entities;
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

    [Fact]
    public async Task SaveAndLoadSnapshot_ReturnsRemainingEvents()
    {
        var store = CreateStore();
        var id = Guid.NewGuid();
        var product = Product.Create(id, "Test");
        await store.SaveAsync(id, product.Events);
        product.ClearEvents();
        await store.SaveSnapshotAsync(id, product);
        await store.SaveAsync(id, new IEvent[] { new ProductSubmitted(id, DateTime.UtcNow) });

        var (snapshot, events) = await store.GetWithSnapshotAsync<Product>(id);

        Assert.NotNull(snapshot);
        Assert.Equal(typeof(Product).AssemblyQualifiedName, snapshot!.AggregateType);
        Assert.Equal(id, snapshot.AggregateId);
        Assert.Equal("Test", snapshot.State.Name);
        Assert.Single(events);
        Assert.Contains(events, e => e is ProductSubmitted);
    }
}
