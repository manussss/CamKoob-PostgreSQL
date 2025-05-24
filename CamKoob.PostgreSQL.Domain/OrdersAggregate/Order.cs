namespace CamKoob.PostgreSQL.Domain.OrdersAggregate;

public class Order : Entity
{
    public string Description { get; private set; } = string.Empty;
    public IEnumerable<OrderItem> OrderItems { get; private set; }

    public Order()
    {
        OrderItems = [];
    }
}