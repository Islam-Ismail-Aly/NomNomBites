using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
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
