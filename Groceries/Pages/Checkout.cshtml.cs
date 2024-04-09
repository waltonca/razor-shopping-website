using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Groceries.Pages
{
    public class CheckoutModel : PageModel
    {
        public int CartSum { get; set; } = 0;
        public void OnGet()
        {
            // Get the existing cookie value
            string? cookieValue = Request.Cookies["ProductIDs"]; 

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

        private void createCookie(string value)
        {
            Response.Cookies.Append("ProductIDs", value, new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            });
        }
    }
}
