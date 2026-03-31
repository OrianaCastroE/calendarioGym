namespace Domain.DTOs.ProductDTOs;

internal class CreateProductDto
{
    // Código no se debería ver...
    // public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ProductLine { get; set; }
    public string? Category { get; set; }
    public string? ImageUrl { get; set; } // A chequear
}
