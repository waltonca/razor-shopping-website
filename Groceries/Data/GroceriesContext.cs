using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Groceries.Models;

namespace Groceries.Data
{
    public class GroceriesContext : DbContext
    {
        public GroceriesContext (DbContextOptions<GroceriesContext> options)
            : base(options)
        {
        }

        public DbSet<Groceries.Models.Grocery> Grocery { get; set; } = default!;
    }
}
