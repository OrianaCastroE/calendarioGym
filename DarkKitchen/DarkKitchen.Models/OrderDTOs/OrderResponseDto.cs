namespace DarkKitchen.Models.OrderDTOs;

public readonly record struct OrderResponseDto(int id, int clientId, string status, DateTime createdAt, decimal subtotal, decimal discount, decimal iva, decimal shippingCost, decimal total, List<OrderProductDto> products);
