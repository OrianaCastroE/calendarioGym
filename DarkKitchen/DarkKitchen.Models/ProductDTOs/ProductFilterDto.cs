namespace DarkKitchen.Models.ProductDTOs;

public readonly record struct ProductFilterDto(string? productLine, List<string>? categories, string? name);
