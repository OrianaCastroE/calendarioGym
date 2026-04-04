namespace DarkKitchen.Models.ProductDTOs;

public class UpdateProductDto
{
    public Guid? Id { get; set; } // Guid o int?
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ProductLine { get; set; }
    public string? Category { get; set; }
    public string[]? ImageUrl { get; set; } // A chequear
    public bool? IsActive { get; set; }
}
