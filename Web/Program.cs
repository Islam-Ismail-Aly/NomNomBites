using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Auto Mapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure the connection string
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Configure password requirements
                options.Password.RequireDigit = false; // Requires a digit (0-9)
                options.Password.RequireLowercase = false; // Requires a lowercase letter (a-z)
                options.Password.RequireUppercase = false; // Requires an uppercase letter (A-Z)
                options.Password.RequireNonAlphanumeric = false; // Does not require a non-alphanumeric character
                options.Password.RequiredLength = 8; // Minimum required password length
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add services UnitOfWork
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}"
                );

                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.Run();
        }
    }
}
