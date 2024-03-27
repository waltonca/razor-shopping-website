using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Groceries.Data;
using Groceries.Models;
using System.ComponentModel;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Groceries.Pages.Admin
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly GroceriesContext _context;
        private readonly IHostEnvironment _environment;

        [BindProperty]
        public Grocery Grocery { get; set; } = default!;

        [BindProperty]
        [DisplayName("Upload Photo")]
        public IFormFile FileUpload { get; set; }
        
        public CreateModel(Groceries.Data.GroceriesContext context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Set the Publish Date for the grocery
            Grocery.PublishDate = DateTime.Now;

            //
            // Upload file to server
            //

            // Make a unique filename
            string filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff_") + FileUpload.FileName;

            // Update Grocery object to include the Grocery filename
            Grocery.ImageFileName = filename;

            // Save the file
            string projectRootPath = _environment.ContentRootPath;
            string fileSavePath = Path.Combine(projectRootPath, "wwwroot","uploads", filename);

            // We use a "using" to ensure the filestream is disposed of when we're done with it
            using (FileStream fileStream = new FileStream(fileSavePath, FileMode.Create))
            {
                FileUpload.CopyTo(fileStream);
            }

            // Update the .net context
            _context.Grocery.Add(Grocery);

            // Sync .net context with database (execute insert command)
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
