using Groceries.Data;
using Groceries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Groceries.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly GroceriesContext _context;

        public IList<Grocery> Groceries { get; set; } = default!;

        // Cookie to store product IDs, "9,3,2,8,9,9"
        public List<int> ProductIDs { get; set; } = new List<int>();
        // It seems that I don't need create ProductIDs cookies in Index page, Add ID only happen in Details page.
        // Index only need to read the cookie, and sum up, display the CartSum
        public int CartSum { get; set; } = 0;
        // Subtotal $28.99
        public decimal Subtotal { get; set; } = 0;
        // Tax $2.31
        public decimal Tax { get; set; } = 0;
        // Shipping $0.00
        public decimal Shipping { get; set; } = 0;
        // Total $31.30
        public decimal Total { get; set; } = 0;

        public class PurchaseData
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Province { get; set; } = string.Empty;
            public string PostalCode { get; set; } = string.Empty;
            public string ccNumber { get; set; } = string.Empty;
            public string ccExpiryDate { get; set; } = string.Empty;
            public string cvv { get; set; } = string.Empty;
            public string products { get; set; } = string.Empty;
        }

        [BindProperty]
        public PurchaseData purchaseData { get; set; } = default!;

        public CheckoutModel(ILogger<IndexModel> logger, GroceriesContext context)
        {
            _logger = logger;
            _context = context;
            purchaseData = new PurchaseData();
        }
        public async Task OnGetAsync()
        {
            //
            // Get the existing cookie value
            string? cookieValue = Request.Cookies["ProductIDs"]; // Can only read the cookie value 1st character, not the whole string


            // Mockup data
            // string? cookieValue = "1,2,3,4,6,44,55";

            // Cookie does not exist
            if (cookieValue == null)
            {
                // Create cookie and set its initial value to 0
                createCookie("");
            }
            else// If the cookie exists, parse its value into ProductIDs list
            {
                // Fix how to get the length of "9,3,2,8,9,9"
                CartSum = cookieValue.Split("-").Length;

                // Parse the value of the cookie and render the page to display a list of the products with details including image. 
                string[] ids = cookieValue.Split("-");

                // Add product IDs to the list
                ProductIDs.AddRange(ids.Select(int.Parse));

                // set purchaseData.products 
                purchaseData.products = cookieValue.Replace("-", ",");

            }
            // Get the products from the database
            Groceries = await _context.Grocery.Where(g => ProductIDs.Contains(g.Id)).ToListAsync();

            // Calculate the total price of the products in the cart
            Subtotal = Groceries.Sum(g => g.Price);
            // Calculate the tax, which is 15% of the subtotal,
            // LIKE $12.59
            Tax = Math.Round(Subtotal * 0.15m, 2);
            // Calculate the shipping cost, let's assume it's free
            // If the subtotal is greater than $50, the shipping is free. Otherwise, the shipping cost is $5.
            if (Subtotal > 50)
            {
                Shipping = 0;
            }
            else
            {
                Shipping = 5;
            }
            // Calculate the total price
            Total = Subtotal + Tax;

        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validate the form
            if (!ModelState.IsValid)
            {
                
                return Page();
            }


            // Serialize purchaseData to JSON
            string jsonData = JsonConvert.SerializeObject(purchaseData);

            // Hard code sample JSON payload
            string jsonPayload = "{\"FirstName\":\"John\",\"LastName\":\"Doe\",\"Address\":\"123 Main St\",\"City\":\"New York\",\"Province\":\"NY\",\"PostalCode\":\"B3L1X6\",\"ccNumber\":\"1111111111111111\",\"ccExpiryDate\":\"1225\",\"cvv\":\"123\",\"products\":\"1,2,3,4,5\"}";

           
            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri("https://purchasesapi20240407212852.azurewebsites.net");
                
                client.DefaultRequestHeaders.Accept.Clear();
                
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    
                    HttpResponseMessage response = await client.PostAsync("/", new StringContent(jsonData, Encoding.UTF8, "application/json"));

                    
                    if (response.IsSuccessStatusCode)
                    {
                        
                        string responseString = await response.Content.ReadAsStringAsync();
                        
                        TempData["InvoiceNumber"] = responseString;
                        // Clear the cookie
                        Response.Cookies.Delete("ProductIDs");

                        return RedirectToPage("./Confirmation");
                    }
                    else
                    {
                        
                        string responseString = await response.Content.ReadAsStringAsync();
                        
                        ModelState.AddModelError("", "An error occurred while processing your request. Please try again later." + responseString);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing your request. Please try again later. Exception: " + ex.Message);
                }
            }

            return Page();

        }

        private void createCookie(string value)
        {
            Response.Cookies.Append("ProductIDs", value, new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            });
        }

        // I need update order summary and CartSum
        private void updateOrder()
        {
            
        }
    }
}
