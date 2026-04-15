namespace DarkKitchen.Models.OrderDTOs;

public readonly record struct OrderDto(string deliveryType, AddressDto address, List<OrderProductDto> products);
