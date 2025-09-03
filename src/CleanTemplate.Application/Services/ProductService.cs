using CleanTemplate.Application.Interfaces;
using CleanTemplate.Domain.Entities;
using CleanTemplate.Domain.Events;

namespace CleanTemplate.Application.Services;

public class ProductService
{
    private readonly IEventStore _eventStore;

    public ProductService(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<Guid> CreateAsync(string name, CancellationToken ct = default)
    {
        var product = Product.Create(Guid.NewGuid(), name);
        await _eventStore.SaveAsync(product.Id, product.Events, ct);
        product.ClearEvents();
        return product.Id;
    }

    private async Task<Product> LoadAsync(Guid id, CancellationToken ct)
    {
        var events = await _eventStore.GetAsync(id, ct);
        return Product.Rehydrate(events);
    }

    public async Task UpdateAsync(Guid id, string name, CancellationToken ct = default)
    {
        var product = await LoadAsync(id, ct);
        product.Update(name);
        await _eventStore.SaveAsync(id, product.Events, ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await LoadAsync(id, ct);
        product.Delete();
        await _eventStore.SaveAsync(id, product.Events, ct);
    }

    public async Task SubmitAsync(Guid id, CancellationToken ct = default)
    {
        var product = await LoadAsync(id, ct);
        product.Submit();
        await _eventStore.SaveAsync(id, product.Events, ct);
    }

    public async Task ApproveAsync(Guid id, CancellationToken ct = default)
    {
        var product = await LoadAsync(id, ct);
        product.Approve();
        await _eventStore.SaveAsync(id, product.Events, ct);
    }

    public async Task RejectAsync(Guid id, string comment, CancellationToken ct = default)
    {
        var product = await LoadAsync(id, ct);
        product.Reject(comment);
        await _eventStore.SaveAsync(id, product.Events, ct);
    }

    public async Task ReturnAsync(Guid id, string comment, CancellationToken ct = default)
    {
        var product = await LoadAsync(id, ct);
        product.Return(comment);
        await _eventStore.SaveAsync(id, product.Events, ct);
    }

    public async Task<IEnumerable<IEvent>> GetEventsAsync(Guid id, CancellationToken ct = default)
        => await _eventStore.GetAsync(id, ct);
}
