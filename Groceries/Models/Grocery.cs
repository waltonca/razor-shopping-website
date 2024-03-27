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

        // For this assignment, we will not create a User model.
        // Instead, we will use a string to represent the user AND hard code the username and password.
        // public string UserName { get; set; } = string.Empty;
    }
}
