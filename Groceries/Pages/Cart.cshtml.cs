using Groceries.Data;
using Groceries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Groceries.Pages
{
    public class CartModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly GroceriesContext _context;

        public IList<Grocery> Groceries { get; set; } = default!;

        // Cookie to store product IDs, "9,3,2,8,9,9"
        public List<int> ProductIDs { get; set; } = new List<int>();
        // It seems that I don't need create ProductIDs cookies in Index page, Add ID only happen in Details page.
        // Index only need to read the cookie, and sum up, display the CartSum
        public int CartSum { get; set; } = 0;


        public CartModel(ILogger<IndexModel> logger, GroceriesContext context)
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
