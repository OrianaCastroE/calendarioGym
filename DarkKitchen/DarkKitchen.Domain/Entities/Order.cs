namespace DarkKitchen.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string DeliveryType { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string DoorNumber { get; set; } = string.Empty;
    public string? Apartment { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Total { get; set; }
    public List<OrderProduct> Products { get; set; } = [];
}
