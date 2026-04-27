namespace DarkKitchen.Models.OrderDTOs;

public readonly record struct MonthlySalesDto(int year, int month, List<ClientSalesLineDto> lines, decimal total);
