namespace DarkKitchen.Models.ProductDTOs;

public readonly record struct CreateProductDto(string? code, string? name, string? description, string? productLine, string? category, decimal? price, string[]? imageUrl);
