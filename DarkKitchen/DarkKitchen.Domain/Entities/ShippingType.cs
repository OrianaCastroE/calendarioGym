namespace DarkKitchen.Domain.Entities;

public class ShippingType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public ShippingType(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}
