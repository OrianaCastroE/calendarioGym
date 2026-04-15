namespace DarkKitchen.Models.PromotionDTOs;

public readonly record struct PromotionDto(string name, int discountPercentage, DateTime dateFrom, DateTime dateTo);
