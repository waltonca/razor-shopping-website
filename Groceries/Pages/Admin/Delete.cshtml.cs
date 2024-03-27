using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Groceries.Data;
using Groceries.Models;
using Microsoft.AspNetCore.Authorization;

namespace Groceries.Pages.Admin
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly Groceries.Data.GroceriesContext _context;

        public DeleteModel(Groceries.Data.GroceriesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Grocery Grocery { get; set; } = default!;

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grocery = await _context.Grocery.FindAsync(id);
            if (grocery != null)
            {
                Grocery = grocery;
                _context.Grocery.Remove(Grocery);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
