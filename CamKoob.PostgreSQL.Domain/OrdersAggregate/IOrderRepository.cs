namespace CamKoob.PostgreSQL.Domain.OrdersAggregate;

public interface IOrderRepository
{
    Task CreateAsync(Order order);
    Task<IEnumerable<Order>?> GetAsync();
    Task<Order?> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, Order newOrder);
}