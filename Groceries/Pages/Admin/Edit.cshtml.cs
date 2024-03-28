using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Groceries.Data;
using Groceries.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
namespace Groceries.Pages.Admin
{
    [Authorize(Roles = "Adminstrator")]
    public class EditModel : PageModel
    {
        private readonly GroceriesContext _context;
        private readonly IHostEnvironment _environment;
        
        [BindProperty]
        public Grocery Grocery { get; set; } = default!;

        [BindProperty]
        [DisplayName("Change Photo")]
        public IFormFile FileUpload { get; set; }

        public EditModel(Groceries.Data.GroceriesContext context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
            Grocery = grocery;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //
            // Upload file to server
            //

            // Make a unique filename
            string filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff_") + FileUpload.FileName;

            // Update Grocery object to include the Grocery filename
            Grocery.ImageFileName = filename;

            // Save the file
            string projectRootPath = _environment.ContentRootPath;
            string fileSavePath = Path.Combine(projectRootPath, "wwwroot", "uploads", filename);

            // We use a "using" to ensure the filestream is disposed of when we're done with it
            using (FileStream fileStream = new FileStream(fileSavePath, FileMode.Create))
            {
                FileUpload.CopyTo(fileStream);
            }

            _context.Attach(Grocery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroceryExists(Grocery.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }
        private bool GroceryExists(int id)
        {
            return _context.Grocery.Any(e => e.Id == id);
        }
    }
}