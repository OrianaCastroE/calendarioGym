namespace DarkKitchen.Models.OrderDTOs;

public readonly record struct ClientSalesLineDto(string clientName, decimal total);
