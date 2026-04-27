namespace DarkKitchen.Models.OrderDTOs;

public readonly record struct SalesReportDto(List<MonthlySalesDto> months, decimal total);
