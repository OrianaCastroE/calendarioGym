namespace DarkKitchen.Models.PromotionDTOs;

public record PromotionFiltersDto
{
    public DateTime? Date { get; init; }
    public string? ProductLine { get; init; }
    public string? ProductName { get; init; }
}
