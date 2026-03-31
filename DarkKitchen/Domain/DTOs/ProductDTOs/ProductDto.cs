namespace Domain.DTOs.ProductDTOs;

public class ProductDto
{
    public Guid? Id { get; set; } // Código identificador
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ProductLine { get; set; }
    public string? Category { get; set; }
    public string[]? ImageUrl { get; set; } // A chequear
}
