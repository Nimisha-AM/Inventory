namespace inventory.Models;

public class Stock
{
    public int Id { get; set; }
    public int VariantId { get; set; }
    public int Quantity { get; set; }

    public Variant Variant { get; set; }
}