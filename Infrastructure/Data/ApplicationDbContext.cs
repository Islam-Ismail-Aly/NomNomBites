using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerFoods> CustomerFoods { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CustomerOrderPayment> CustomerOrderPayments { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodOrders> FoodOrders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Configure composite primary key using HasKey method

            #region Composite Primary Key for FoodOrders

            builder.Entity<FoodOrders>().HasKey(t => new { t.FoodId, t.OrderId });

            #endregion

            #region Composite Primary Key for CustomerFoods

            builder.Entity<CustomerFoods>().HasKey(t => new { t.FoodId, t.CustomerId });

            #endregion

            #region Composite Primary Key for CustomerOrderPayment

            builder.Entity<CustomerOrderPayment>().HasKey(t => new { t.OrderId, t.CustomerId, t.PaymentId });

            #endregion
        }
    }
}
