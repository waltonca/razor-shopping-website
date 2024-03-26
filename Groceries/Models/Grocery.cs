namespace Groceries.Models
{
    public class Grocery
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageFileName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
