using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Customer GetByUserId(string userId)
        {
            var customer = _dbContext.Customers.FirstOrDefault(c => c.ApplicationUserId == userId);
            return customer;
        }

        public IEnumerable<Order> GetOrdersByCustomerId(int customerId)
        {
            var orders = _dbContext.Customers
                                   .Include(c => c.Orders)
                                   .FirstOrDefault(c => c.Id == customerId)
                                   ?.Orders;

            return orders ?? Enumerable.Empty<Order>();
        }
    }
}
