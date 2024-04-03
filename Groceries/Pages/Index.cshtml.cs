using Groceries.Data;
using Groceries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Groceries.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly GroceriesContext _context;

        public IList<Grocery> Groceries { get; set; } = default!;

        public IndexModel(ILogger<IndexModel> logger, GroceriesContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task OnGetAsync()
        {
            Groceries = await _context.Grocery.ToListAsync();
        }
    }
}
