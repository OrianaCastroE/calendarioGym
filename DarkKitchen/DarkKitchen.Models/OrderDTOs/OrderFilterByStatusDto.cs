namespace DarkKitchen.Models.OrderDTOs;

public readonly record struct OrderFilterByStatusDto(DateTime dateFrom, DateTime dateTo, string? address, string? status);
