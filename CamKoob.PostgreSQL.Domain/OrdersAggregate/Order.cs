namespace CamKoob.PostgreSQL.Domain.OrdersAggregate;

public class Order : Entity
{
    public string Description { get; private set; } = string.Empty;
    public List<OrderItem> OrderItems { get; private set; }

    public Order()
    {
        OrderItems = [];
    }

    public Order(string description, List<OrderItem> orderItems)
    {
        Description = description;
        OrderItems = orderItems;
    }

    [JsonConstructor]
    public Order(Guid id, DateTime createdAt, string description, List<OrderItem> orderItems)
    {
        Id = id;
        CreatedAt = createdAt;
        Description = description;
        OrderItems = orderItems;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public void SetItems(List<OrderItem> items)
    {
        OrderItems = items;
    }
}