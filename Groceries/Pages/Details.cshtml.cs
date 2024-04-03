using Groceries.Data;
using Groceries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Groceries.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly GroceriesContext _context;

        public Grocery Grocery { get; set; } = default!;

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
            }
            return Page();
        }
    }
}
