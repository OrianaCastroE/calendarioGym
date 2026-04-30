namespace DarkKitchen.Models.OrderDTOs;

public readonly record struct OrderFiltersDto(DateTime? dateFrom, DateTime? dateTo, string? status);
