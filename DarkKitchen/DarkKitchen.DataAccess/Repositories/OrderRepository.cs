using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;

namespace DarkKitchen.DataAccess.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    private readonly AppDbContext context = context;

    public void Add(Order order)
    {
        context.Orders.Add(order);
        context.SaveChanges();
    }

    public Order? GetById(int id)
    {
        return context.Orders.Find(id);
    }

    public IEnumerable<Order> GetClientOrders(int clientId, DateTime? dateFrom, DateTime? dateTo, string? status)
    {
        var query = context.Orders.Where(o => o.ClientId == clientId);

        if(dateFrom.HasValue)
        {
            query = query.Where(o => o.CreatedAt >= dateFrom);
        }

        if(dateTo.HasValue)
        {
            query = query.Where(o => o.CreatedAt <= dateTo);
        }

        if(!string.IsNullOrEmpty(status))
        {
            query = query.Where(o => o.Status == status);
        }

        return query.ToList();
    }

    public IEnumerable<Order> GetOrdersByStatus(DateTime dateFrom, DateTime dateTo, string? address, string? status)
    {
        var query = context.Orders.Where(o => o.CreatedAt >= dateFrom && o.CreatedAt <= dateTo);

        if(!string.IsNullOrEmpty(address))
        {
            query = query.Where(o => o.Street.Contains(address));
        }

        if(!string.IsNullOrEmpty(status))
        {
            query = query.Where(o => o.Status == status);
        }

        return query.ToList();
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
        context.SaveChanges();
    }
}
