namespace Core.Interfaces.DTOs;

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string PictureUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}