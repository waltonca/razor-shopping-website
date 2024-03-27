using Groceries.Data;
using Groceries.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Groceries.Pages.Admin
{
    public class LoginModel : PageModel
    {

        private readonly GroceriesContext _context;

        [BindProperty]
        public string UserName { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public LoginModel(GroceriesContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost() 
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Hard code the username and password
            if (UserName != "admin" || Password != "nscc123")
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return Page();
            }
            else
            {
                // Initialize the Cookie session and Redirect to the Admin/Index page
                
                // Create Claims
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim(ClaimTypes.Role, "Adminstrator")
                };

                //Create Identity
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign in user
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties());

                return RedirectToPage("/Admin/Index");
            }
           
        }
    }
}
