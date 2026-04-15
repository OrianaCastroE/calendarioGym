namespace DarkKitchen.Models.PromotionDTOs;

public readonly record struct PromotionFiltersDto(DateTime? date, string? productLine, string? productName);
