using Groceries.Data;
using Groceries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Groceries.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly GroceriesContext _context;

        public IList<Grocery> Groceries { get; set; } = default!;

        // Cookie to store product IDs, "9,3,2,8,9,9"
        public List<int> ProductIDs { get; set; } = new List<int>();
        // It seems that I don't need create ProductIDs cookies in Index page, Add ID only happen in Details page.
        // Index only need to read the cookie, and sum up, display the CartSum
        public int CartSum { get; set; } = 0;
        // Subtotal $28.99
        public decimal Subtotal { get; set; } = 0;
        // Tax $2.31
        public decimal Tax { get; set; } = 0;
        // Shipping $0.00
        public decimal Shipping { get; set; } = 0;
        // Total $31.30
        public decimal Total { get; set; } = 0;


        public CheckoutModel(ILogger<IndexModel> logger, GroceriesContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task OnGetAsync()
        {
            //
            // Get the existing cookie value
            string? cookieValue = Request.Cookies["ProductIDs"]; // Can only read the cookie value 1st character, not the whole string


            // Mockup data
            // string? cookieValue = "1,2,3,4,6,44,55";

            // Cookie does not exist
            if (cookieValue == null)
            {
                // Create cookie and set its initial value to 0
                createCookie("");
            }
            else// If the cookie exists, parse its value into ProductIDs list
            {
                // Fix how to get the length of "9,3,2,8,9,9"
                CartSum = cookieValue.Split("-").Length;

                // Parse the value of the cookie and render the page to display a list of the products with details including image. 
                string[] ids = cookieValue.Split("-");

                // Add product IDs to the list
                ProductIDs.AddRange(ids.Select(int.Parse));


            }
            // Get the products from the database
            Groceries = await _context.Grocery.Where(g => ProductIDs.Contains(g.Id)).ToListAsync();

            // Calculate the total price of the products in the cart
            Subtotal = Groceries.Sum(g => g.Price);
            // Calculate the tax, which is 15% of the subtotal,
            // LIKE $12.59
            Tax = Math.Round(Subtotal * 0.15m, 2);
            // Calculate the shipping cost, let's assume it's free
            // If the subtotal is greater than $50, the shipping is free. Otherwise, the shipping cost is $5.
            if (Subtotal > 50)
            {
                Shipping = 0;
            }
            else
            {
                Shipping = 5;
            }
            // Calculate the total price
            Total = Subtotal + Tax;

        }

        private void createCookie(string value)
        {
            Response.Cookies.Append("ProductIDs", value, new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            });
        }
    }
}