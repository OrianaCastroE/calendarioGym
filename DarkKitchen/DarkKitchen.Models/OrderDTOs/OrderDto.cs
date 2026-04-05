namespace DarkKitchen.Models.OrderDTOs;

public class OrderDto
{
    public string DeliveryType { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = new();
    public List<OrderProductDto> Products { get; set; } = [];
}
