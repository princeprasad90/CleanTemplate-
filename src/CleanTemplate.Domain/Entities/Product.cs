using CleanTemplate.Domain.Events;

namespace CleanTemplate.Domain.Entities;

public enum ProductStatus
{
    Draft,
    PendingApproval,
    Approved,
    Rejected,
    Returned,
    Deleted
}

public class Product : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public ProductStatus Status { get; private set; } = ProductStatus.Draft;

    private Product() { }

    public static Product Create(Guid id, string name)
    {
        var product = new Product();
        var evt = new ProductCreated(id, name, DateTime.UtcNow);
        product.Apply(evt);
        product.AddEvent(evt);
        return product;
    }

    public static Product Rehydrate(IEnumerable<IEvent> events)
    {
        var product = new Product();
        foreach (var e in events)
        {
            product.Apply(e);
        }
        return product;
    }

    public void Update(string name)
    {
        var evt = new ProductUpdated(Id, name, DateTime.UtcNow);
        Apply(evt);
        AddEvent(evt);
    }

    public void Delete()
    {
        var evt = new ProductDeleted(Id, DateTime.UtcNow);
        Apply(evt);
        AddEvent(evt);
    }

    public void Submit()
    {
        if (Status == ProductStatus.PendingApproval)
            return;
        var evt = new ProductSubmitted(Id, DateTime.UtcNow);
        Apply(evt);
        AddEvent(evt);
    }

    public void Approve()
    {
        if (Status != ProductStatus.PendingApproval) return;
        var evt = new ProductApproved(Id, DateTime.UtcNow);
        Apply(evt);
        AddEvent(evt);
    }

    public void Reject(string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment is required", nameof(comment));
        if (Status != ProductStatus.PendingApproval)
            throw new InvalidOperationException("Product not pending approval");
        var evt = new ProductRejected(Id, comment, DateTime.UtcNow);
        Apply(evt);
        AddEvent(evt);
    }

    public void Return(string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment is required", nameof(comment));
        if (Status != ProductStatus.PendingApproval)
            throw new InvalidOperationException("Product not pending approval");
        var evt = new ProductReturned(Id, comment, DateTime.UtcNow);
        Apply(evt);
        AddEvent(evt);
    }

    private void Apply(IEvent @event)
    {
        switch (@event)
        {
            case ProductCreated created:
                Id = created.ProductId;
                Name = created.Name;
                Status = ProductStatus.Draft;
                break;
            case ProductUpdated updated:
                Name = updated.Name;
                break;
            case ProductDeleted:
                Status = ProductStatus.Deleted;
                break;
            case ProductSubmitted:
                Status = ProductStatus.PendingApproval;
                break;
            case ProductApproved:
                Status = ProductStatus.Approved;
                break;
            case ProductRejected:
                Status = ProductStatus.Rejected;
                break;
            case ProductReturned:
                Status = ProductStatus.Returned;
                break;
        }
    }
}
