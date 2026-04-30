namespace DarkKitchen.Models.OrderDTOs;

public readonly record struct AddressDto(string street, string doorNumber, string? apartment);
