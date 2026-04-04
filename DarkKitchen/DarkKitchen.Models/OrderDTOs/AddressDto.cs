namespace DarkKitchen.Models.OrderDTOs;

public class AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string DoorNumber { get; set; } = string.Empty;
    public string? Apartment { get; set; }
}
