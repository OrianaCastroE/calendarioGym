namespace DarkKitchen.Models.ProductDTOs;

public readonly record struct ProductFilter(string? productLine, List<string>? categories, string? name);
