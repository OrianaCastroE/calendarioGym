using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DarkKitchen.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Promotion> Promotion { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RolePermissions> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RolePermissions>(b =>
        {
            b.HasKey(r => r.Role);

            b.HasData(
                new RolePermissions
                {
                    Role = Role.Admin,
                    Permissions =
                    [
                        Permission.CreateUser,
                        Permission.UpdateUser,
                        Permission.DeleteUser,
                        Permission.GetUsers,
                        Permission.CreateProduct,
                        Permission.UpdateProduct,
                        Permission.UpdateProductStatus,
                        Permission.GetProducts,
                        Permission.CreatePromotion,
                        Permission.UpdatePromotion,
                        Permission.UpdatePromotionProducts,
                        Permission.GetCurrentPromotions,
                        Permission.GetOrderDetails,
                        Permission.GetMostPopularProducts,
                        Permission.GetSellsReport
                    ]
                },
                new RolePermissions
                {
                    Role = Role.Client,
                    Permissions =
                    [
                        Permission.GetProducts,
                        Permission.GetCurrentPromotions,
                        Permission.PlaceOrder,
                        Permission.GetMyOrders
                    ]
                },
                new RolePermissions
                {
                    Role = Role.Dispatcher,
                    Permissions =
                    [
                        Permission.GetOrdersByStatus,
                        Permission.GetOrderDetails,
                        Permission.SetOrderStatusToPrepared,
                        Permission.SetOrderStatusToCanceled,
                        Permission.SetOrderStatusToOnItsWay,
                        Permission.SetOrderStatusToDelivered,
                        Permission.SetOrderStatusToNotDelivered
                    ]
                });
        });

        modelBuilder.Entity<User>()
            .HasOne<RolePermissions>()
            .WithMany()
            .HasForeignKey(u => u.Role)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
