namespace CamKoob.PostgreSQL.Infra.Repositories;

public class OrderRepository(IConfiguration configuration) : IOrderRepository
{
    public async Task CreateAsync(Order order)
    {
        using var conn = new NpgsqlConnection(configuration.GetConnectionString("Database"));

        var sql = @"
            INSERT INTO orders (id, order)
            VALUES (@Id, to_jsonb(@Order::json))
        ";

        await conn.ExecuteAsync(sql, new { id = order.Id, order = order });
    }

    public async Task<IEnumerable<Order>> GetAsync()
    {
        using var conn = new NpgsqlConnection(configuration.GetConnectionString("Database"));

        var sql = "SELECT order FROM orders";
        var orders = await conn.QueryAsync<string>(sql);

        return orders.Select(json => System.Text.Json.JsonSerializer.Deserialize<Order>(json)!);
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        using var conn = new NpgsqlConnection(configuration.GetConnectionString("Database"));

        var sql = "SELECT order FROM orders WHERE id = @id";
        var json = await conn.QueryFirstOrDefaultAsync<string>(sql, new { id = id });

        if (string.IsNullOrEmpty(json))
            return null;

        return System.Text.Json.JsonSerializer.Deserialize<Order>(json);
    }

    public async Task<bool> UpdateAsync(Guid id, Order newOrder)
    {
        using var conn = new NpgsqlConnection(configuration.GetConnectionString("Database"));

        var selectSql = "SELECT order FROM orders WHERE id = @id";
        var json = await conn.QueryFirstOrDefaultAsync<string>(selectSql, new { id = id });

        if (json is null)
            return false;

        var order = System.Text.Json.JsonSerializer.Deserialize<Order>(json)!;

        if (!string.IsNullOrWhiteSpace(newOrder.Description))
            order.SetDescription(newOrder.Description);

        if (newOrder.OrderItems != null && newOrder.OrderItems.Count != 0)
            order.AddItems(newOrder.OrderItems);

        var updateSql = @"
            UPDATE pedidos SET conteudo = to_jsonb(@order::json) WHERE id = @Id
        ";

        await conn.ExecuteAsync(updateSql, new { id = id, order = order });

        return true;
    }
}