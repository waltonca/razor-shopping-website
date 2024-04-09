using Groceries.Data;
using Groceries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Groceries.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly GroceriesContext _context;

        public Grocery Grocery { get; set; } = default!;
        public List<int> ProductIDs { get; set; } = new List<int>();
        public int CartSum { get; set; } = 0;

        // Constructor
        public DetailsModel(GroceriesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grocery = await _context.Grocery.FirstOrDefaultAsync(m => m.Id == id);

            if (grocery == null)
            {
                return NotFound();
            }
            else
            {
                Grocery = grocery;

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

                }
            }
            return Page();
        }


        public IActionResult OnPost()
        {
            // Check if the form has been submitted with actual form data
            if (HttpContext.Request.Form.Count > 0)
            {
                // Get the existing cookie value
                string? cookieValue = Request.Cookies["ProductIDs"];

                // Get the product ID from the form
                string productId = HttpContext.Request.Form["id"];

                if (cookieValue != null)
                {
                    // If initial value is empty string, set cookieValue to productId directly
                    if (cookieValue == "")
                    {
                        cookieValue = productId;
                    }
                    else
                    {
                        // Add the new product ID to the existing cookie
                        cookieValue += "-" + productId;
                    }

                    // Update the cookie with the new value
                    createCookie(cookieValue);
                }
                else
                {
                    // If cookie doesn't exist, create it with the productId
                    createCookie(productId);
                }
            }

            return RedirectToPage("Index");
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
