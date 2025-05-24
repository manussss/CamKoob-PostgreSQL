namespace CamKoob.PostgreSQL.Domain.OrdersAggregate;

public class Order : Entity
{
    public string Description { get; private set; } = string.Empty;
    public List<OrderItem> OrderItems { get; private set; }

    public Order()
    {
        OrderItems = [];
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public void AddItems(List<OrderItem> items)
    {
        OrderItems.AddRange(items);
    }
}