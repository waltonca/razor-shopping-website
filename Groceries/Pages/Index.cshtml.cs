using Groceries.Data;
using Groceries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Linq;

namespace Groceries.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly GroceriesContext _context;

        public IList<Grocery> Groceries { get; set; } = default!;
        
        // Cookie to store product IDs, "9,3,2,8,9,9"
        public List<int> ProductIDs { get; set; } = new List<int>();
        // It seems that I don't need create ProductIDs cookies in Index page, Add ID only happen in Details page.
        // Index only need to read the cookie, and sum up, display the CartSum
        public int CartSum { get; set; } = 0;

       
        public IndexModel(ILogger<IndexModel> logger, GroceriesContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task OnGetAsync()
        {
            Groceries = await _context.Grocery.ToListAsync();
            //
            // Get the existing cookie value
            string? cookieValue = Request.Cookies["ProductIDs"];
            // Mockup data
            // string? cookieValue = "1,2,3,4,6,44,55";

            // Cookie does not exist
            if (cookieValue == null)
            {
                // Create cookie and set its initial value to 0
                createCookie(0);
            }
            else// If the cookie exists, parse its value into ProductIDs list
            {
                // Fix how to get the length of "9,3,2,8,9,9"
                
                CartSum = cookieValue.Split(",").Length;

            }
        }



        // A helper function to create a cookie and set its value to count
        private void createCookie(int count)
        {
            Response.Cookies.Append("ProductIDs", count.ToString(), new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            });
        }

    }
}
