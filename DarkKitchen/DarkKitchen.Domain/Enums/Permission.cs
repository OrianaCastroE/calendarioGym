namespace DarkKitchen.Domain.Enums;

public enum Permission
{
    GetProducts,
    GetCurrentPromotions,
    PlaceOrder,
    GetMyOrders,
    GetOrdersByStatus,
    GetOrderDetails,
    SetOrderStatusToPrepared,
    SetOrderStatusToCanceled,
    SetOrderStatusToOnItsWay,
    SetOrderStatusToDelivered,
    SetOrderStatusToNotDelivered,
    CreateUser,
    UpdateUser,
    DeleteUser,
    GetUsers,
    CreateProduct,
    UpdateProduct,
    CreatePromotion,
    UpdatePromotion,
    GetMostPopularProducts,
    GetSellsReport
}
