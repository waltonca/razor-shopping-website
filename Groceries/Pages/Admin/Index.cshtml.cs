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
    [Authorize(Roles = "Adminstrator")]
    public class IndexModel : PageModel
    {
        private readonly Groceries.Data.GroceriesContext _context;

        public IndexModel(Groceries.Data.GroceriesContext context)
        {
            _context = context;
        }

        public IList<Grocery> Grocery { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Grocery = await _context.Grocery.ToListAsync();
        }
    }
}
