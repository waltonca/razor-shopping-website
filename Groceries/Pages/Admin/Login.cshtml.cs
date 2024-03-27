using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Groceries.Pages.Admin
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        public string UserName { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost() 
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage("/Admin/Index");
            
        }
    }
}
