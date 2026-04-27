using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Models.OrderDTOs;

namespace DarkKitchen.Services;

public class OrderService(IOrderRepository orderRepository, IUserService userService) : IOrderService
{
    private readonly IOrderRepository orderRepository = orderRepository;
    private readonly IUserService userService = userService;

    public OrderResponseDto CreateOrder(OrderDto newOrder)
    {
        if(newOrder.products.Count == 0)
        {
            throw new Exception("Order must have at least one product.");
        }

        if(newOrder.deliveryType != "express" && newOrder.deliveryType != "24hs")
        {
            throw new Exception("Invalid delivery type.");
        }

        if(string.IsNullOrEmpty(newOrder.address.street))
        {
            throw new Exception("Street cannot be empty.");
        }

        if(string.IsNullOrEmpty(newOrder.address.doorNumber))
        {
            throw new Exception("Door number cannot be empty.");
        }

        var order = new Order()
        {
            DeliveryType = newOrder.deliveryType,
            Street = newOrder.address.street,
            DoorNumber = newOrder.address.doorNumber,
            Apartment = newOrder.address.apartment,
            Status = "Pending",
            CreatedAt = DateTime.Now
        };

        orderRepository.Add(order);

        return new OrderResponseDto(order.Id, order.ClientId, order.Status, order.CreatedAt, order.Subtotal, order.ShippingCost, order.Total, []);
    }

    public List<OrderResponseDto> GetClientOrders(int clientId, OrderFiltersDto filter)
    {
        var orders = orderRepository.GetClientOrders(clientId, filter.dateFrom, filter.dateTo, filter.status);

        return orders.Select(o => new OrderResponseDto(o.Id, o.ClientId, o.Status, o.CreatedAt, o.Subtotal, o.ShippingCost, o.Total, [])).ToList();
    }

    public List<OrderResponseDto> GetOrdersByStatus(OrderFilterByStatusDto filter)
    {
        var orders = orderRepository.GetOrdersByStatus(filter.dateFrom, filter.dateTo, filter.address, filter.status);

        return orders.Select(o => new OrderResponseDto(o.Id, o.ClientId, o.Status, o.CreatedAt, o.Subtotal, o.ShippingCost, o.Total, [])).ToList();
    }

    public OrderResponseDto GetOrderById(int orderId)
    {
        var order = orderRepository.GetById(orderId)
            ?? throw new Exception("Order not found.");

        return new OrderResponseDto(order.Id, order.ClientId, order.Status, order.CreatedAt, order.Subtotal, order.ShippingCost, order.Total, []);
    }

    public void UpdateOrderStatus(int orderId, UpdateOrderStatusDto newStatus, List<Permission> userPermissions)
    {
        if(!Enum.TryParse<OrderStatus>(newStatus.status, out var status))
        {
            throw new BadRequestException("Invalid order status.");
        }

        var requiredPermission = status switch
        {
            OrderStatus.Prepared => Permission.SetOrderStatusToPrepared,
            OrderStatus.Canceled => Permission.SetOrderStatusToCanceled,
            OrderStatus.OnItsWay => Permission.SetOrderStatusToOnItsWay,
            OrderStatus.Delivered => Permission.SetOrderStatusToDelivered,
            OrderStatus.NotDelivered => Permission.SetOrderStatusToNotDelivered,
            _ => throw new BadRequestException("Invalid order status.")
        };

        if(!userPermissions.Contains(requiredPermission))
        {
            throw new AccessDeniedException("User does not have permission to set this order status.");
        }

        var order = orderRepository.GetById(orderId)
            ?? throw new NotFoundException("Order not found.");

        if(!Enum.TryParse<OrderStatus>(order.Status, out var currentStatus))
        {
            throw new BadRequestException("Invalid current order status.");
        }

        var validTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
    {
        { OrderStatus.Pending, [OrderStatus.Prepared, OrderStatus.Canceled] },
        { OrderStatus.Prepared, [OrderStatus.OnItsWay] },
        { OrderStatus.OnItsWay, [OrderStatus.Delivered, OrderStatus.NotDelivered] },
        { OrderStatus.Canceled, [] },
        { OrderStatus.Delivered, [] },
        { OrderStatus.NotDelivered, [] }
    };

        if(!validTransitions[currentStatus].Contains(status))
        {
            throw new BadRequestException($"Cannot change order status from {currentStatus} to {status}.");
        }

        order.Status = status.ToString();

        orderRepository.Update(order);
    }

    public SalesReportDto GetSalesReport()
    {
        var orders = orderRepository.GetAll().ToList();

        var aggregated = new Dictionary<(int Year, int Month), Dictionary<string, decimal>>();

        foreach(var order in orders)
        {
            var user = userService.GetUserById(order.ClientId)!.Value;
            var clientName = $"{user.name} {user.surname}";
            var key = (order.CreatedAt.Year, order.CreatedAt.Month);

            if(!aggregated.ContainsKey(key))
            {
                aggregated[key] = [];
            }

            if(!aggregated[key].ContainsKey(clientName))
            {
                aggregated[key][clientName] = 0;
            }

            aggregated[key][clientName] += order.Total;
        }

        var months = new List<MonthlySalesDto>();
        decimal grandTotal = 0;

        foreach(var monthEntry in aggregated.OrderBy(m => m.Key.Year).ThenBy(m => m.Key.Month))
        {
            var lines = new List<ClientSalesLineDto>();
            decimal monthTotal = 0;

            foreach(var clientEntry in monthEntry.Value.OrderBy(c => c.Key))
            {
                lines.Add(new ClientSalesLineDto(clientEntry.Key, clientEntry.Value));
                monthTotal += clientEntry.Value;
            }

            months.Add(new MonthlySalesDto(monthEntry.Key.Year, monthEntry.Key.Month, lines, monthTotal));
            grandTotal += monthTotal;
        }

        return new SalesReportDto(months, grandTotal);
    }
}
