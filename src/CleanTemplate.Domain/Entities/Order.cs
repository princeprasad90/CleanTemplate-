using CleanTemplate.Domain.Events;

namespace CleanTemplate.Domain.Entities;

public class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() {}

    public static Order Create(Guid id)
    {
        var order = new Order();
        order.Apply(new OrderCreated(id, DateTime.UtcNow));
        order.AddEvent(new OrderCreated(id, DateTime.UtcNow));
        return order;
    }

    public static Order Rehydrate(IEnumerable<IEvent> events)
    {
        var order = new Order();
        foreach (var e in events)
        {
            order.Apply(e);
        }
        return order;
    }

    public void AddItem(string product, int quantity)
    {
        var evt = new ItemAdded(Id, product, quantity, DateTime.UtcNow);
        Apply(evt);
        AddEvent(evt);
    }

    private void Apply(IEvent @event)
    {
        switch (@event)
        {
            case OrderCreated created:
                Id = created.OrderId;
                break;
            case ItemAdded added:
                _items.Add(new OrderItem(added.Product, added.Quantity));
                break;
        }
    }
}

public record OrderItem(string Product, int Quantity);
