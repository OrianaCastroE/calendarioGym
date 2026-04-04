namespace Models.PromotionDTOs;

public class PromotionResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DiscountPercentage { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}
