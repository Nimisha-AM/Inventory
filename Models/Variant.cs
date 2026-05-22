namespace inventory.Models
{
    public class Variant
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string Size { get; set; }
        public string Color { get; set; }

        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}