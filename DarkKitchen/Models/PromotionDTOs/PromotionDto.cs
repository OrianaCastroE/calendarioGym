namespace Models.PromotionDTOs;

public class PromotionDto
{
    public string Name { get; set; } = string.Empty;
    public int DiscountPercentage { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}
