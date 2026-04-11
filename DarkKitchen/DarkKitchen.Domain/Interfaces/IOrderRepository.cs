using DarkKitchen.Domain.Entities;

namespace DarkKitchen.Domain.Interfaces;

public interface IOrderRepository
{
    void Add(Order order);
    Order? GetById(int id);
    IEnumerable<Order> GetClientOrders(int clientId, DateTime? dateFrom, DateTime? dateTo, string? status);
    IEnumerable<Order> GetOrdersByStatus(DateTime dateFrom, DateTime dateTo, string? address, string? status);
    void Update(Order order);
    void Save();
}
