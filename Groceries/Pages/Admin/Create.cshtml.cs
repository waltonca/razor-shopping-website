using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Groceries.Data;
using Groceries.Models;

namespace Groceries.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly Groceries.Data.GroceriesContext _context;

        public CreateModel(Groceries.Data.GroceriesContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Grocery Grocery { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Grocery.Add(Grocery);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
