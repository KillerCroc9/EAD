using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Furniture.Data.FurnitureContext;
using Furniture.Areas.Identity.Data;
using Furniture.Helper;
namespace Furniture
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("AuthDBConnection") ?? throw new InvalidOperationException("Connection string 'AuthDBConnection' not found.");

            builder.Services.AddDbContext<AuthDB>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDefaultIdentity<FurnitureUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthDB>();
            //builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
            builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });*/
            app.MapRazorPages();
            app.Run();
        }
    }
}