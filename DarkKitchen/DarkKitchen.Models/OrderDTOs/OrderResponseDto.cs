namespace DarkKitchen.Models.OrderDTOs;

public class OrderResponseDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Total { get; set; }
    public List<OrderProductDto> Products { get; set; } = [];
}
