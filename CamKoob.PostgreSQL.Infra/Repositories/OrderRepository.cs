namespace CamKoob.PostgreSQL.Infra.Repositories;

public class OrderRepository(IConfiguration configuration) : IOrderRepository
{
    public async Task CreateAsync(Order order)
    {
        using var conn = new NpgsqlConnection(configuration.GetConnectionString("Database"));

        var sql = @"
            INSERT INTO orders (id, content)
            VALUES (@id, to_jsonb(@content::json))
        ";

        await conn.ExecuteAsync(sql, new { id = order.Id, content = JsonSerializer.Serialize(order) });
    }

    public async Task<IEnumerable<Order>?> GetAsync()
    {
        using var conn = new NpgsqlConnection(configuration.GetConnectionString("Database"));

        var sql = "SELECT content FROM Orders";
        var orders = await conn.QueryAsync<string>(sql);

        if (orders is null || !orders.Any())
            return null;

        return orders.Select(json => JsonSerializer.Deserialize<Order>(json)!);
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        using var conn = new NpgsqlConnection(configuration.GetConnectionString("Database"));

        var sql = "SELECT content FROM orders WHERE id = @id";
        var json = await conn.QueryFirstOrDefaultAsync<string>(sql, new { id = id });

        if (string.IsNullOrEmpty(json))
            return null;

        return JsonSerializer.Deserialize<Order>(json);
    }

    public async Task UpdateAsync(Guid id, Order newOrder)
    {
        using var conn = new NpgsqlConnection(configuration.GetConnectionString("Database"));

        var selectSql = "SELECT content FROM orders WHERE id = @id";
        var json = await conn.QueryFirstOrDefaultAsync<string>(selectSql, new { id = id });

        if (json is null)
            return;

        var order = JsonSerializer.Deserialize<Order>(json)!;

        if (!string.IsNullOrWhiteSpace(newOrder.Description))
            order.SetDescription(newOrder.Description);

        if (newOrder.OrderItems != null && newOrder.OrderItems.Count != 0)
            order.SetItems(newOrder.OrderItems);

        var updateSql = @"
            UPDATE orders SET content = to_jsonb(@content::json) WHERE id = @id
        ";

        await conn.ExecuteAsync(updateSql, new { id = id, content = JsonSerializer.Serialize(order) });
    }
}