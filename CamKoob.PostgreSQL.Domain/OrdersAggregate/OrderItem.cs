namespace CamKoob.PostgreSQL.Domain.OrdersAggregate;

public class OrderItem : Entity
{
    public EType Type { get; private set; }
}