namespace DarkKitchen.Domain.Entities;

public class Address
{
    public required string Street { get; set; }
    public required string DoorNumber { get; set; }
    public string? Apartment { get; set; }
}
