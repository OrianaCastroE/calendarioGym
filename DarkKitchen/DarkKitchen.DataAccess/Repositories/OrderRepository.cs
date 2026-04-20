using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;

namespace DarkKitchen.DataAccess.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    private readonly AppDbContext context = context;

    public void Add(Order order)
    {
        context.Orders.Add(order);
    }

    public Order? GetById(int id)
    {
        return context.Orders.Find(id);
    }

    public IEnumerable<Order> GetClientOrders(int clientId, DateTime? dateFrom, DateTime? dateTo, string? status)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Order> GetOrdersByStatus(DateTime dateFrom, DateTime dateTo, string? address, string? status)
    {
        throw new NotImplementedException();
    }

    public void Update(Order order)
    {
        var existingOrder = context.Orders.Find(order.Id);
        if(existingOrder == null)
        {
            throw new Exception("Order not found");
        }

        context.Entry(existingOrder).CurrentValues.SetValues(order);
        context.SaveChanges();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }
}
