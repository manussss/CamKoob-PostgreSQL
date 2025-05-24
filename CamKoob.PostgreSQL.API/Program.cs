var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositories();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/v1/orders", async (
    [FromBody] OrderDTO order,
    [FromServices] IOrderRepository orderRepository
) =>
{
    await orderRepository.CreateAsync(new Order(order.Description, order.OrderItems));

    return Results.Created();
});

app.MapGet("/api/v1/orders", async (
    [FromServices] IOrderRepository orderRepository
) =>
{
    var orders = await orderRepository.GetAsync();

    if (orders is null || !orders.Any())
        return Results.NotFound();

    return Results.Ok(orders);
});

app.MapGet("/api/v1/orders/{id}", async (
    [FromRoute] Guid id,
    [FromServices] IOrderRepository orderRepository
) =>
{
    var order = await orderRepository.GetByIdAsync(id);

    if (order is null)
        return Results.NotFound();

    return Results.Ok(order);
});

app.MapPatch("/api/v1/orders/{id}", async (
    Guid id,
    [FromBody] OrderDTO newOrder,
    [FromServices] IOrderRepository orderRepository
) =>
{
    await orderRepository.UpdateAsync(id, new Order(newOrder.Description, newOrder.OrderItems));

    return Results.Ok();
});

await app.RunAsync();