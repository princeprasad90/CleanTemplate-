using CleanTemplate.Application.Interfaces;
using CleanTemplate.Domain.Entities;
using CleanTemplate.Domain.Events;

namespace CleanTemplate.Application.Services;

public class OrderService
{
    private readonly IEventStore _eventStore;

    public OrderService(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<Guid> CreateAsync(CancellationToken ct = default)
    {
        var order = Order.Create(Guid.NewGuid());
        await _eventStore.SaveAsync(order.Id, order.Events, ct);
        order.ClearEvents();
        return order.Id;
    }

    public async Task AddItemAsync(Guid orderId, string product, int quantity, CancellationToken ct = default)
    {
        var existing = await _eventStore.GetAsync(orderId, ct);
        var order = Order.Rehydrate(existing);
        order.AddItem(product, quantity);
        await _eventStore.SaveAsync(orderId, order.Events, ct);
    }

    public async Task<IEnumerable<IEvent>> GetEventsAsync(Guid orderId, CancellationToken ct = default) =>
        await _eventStore.GetAsync(orderId, ct);
}
