namespace CamKoob.PostgreSQL.Application.DTOs;

public class OrderDTO
{
    public string Description { get; set; } = string.Empty;
    public List<OrderItem> OrderItems { get; set; } = [];
}