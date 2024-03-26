using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Groceries.Data;
using Groceries.Models;

namespace Groceries.Pages.Admin
{
    public class DetailsModel : PageModel
    {
        private readonly Groceries.Data.GroceriesContext _context;

        public DetailsModel(Groceries.Data.GroceriesContext context)
        {
            _context = context;
        }

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
    }
}
