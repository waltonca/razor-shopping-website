using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Groceries.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace Groceries
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<GroceriesContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("GroceriesContext") ?? throw new InvalidOperationException("Connection string 'GroceriesContext' not found.")));

            // Add services to the container.
            builder.Services.AddRazorPages();


            // Cookie-based authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.LoginPath = "/Admin/Login";
                    options.LogoutPath = "/Admin/Logout";
                    options.AccessDeniedPath = "/AccessDenied/";
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
