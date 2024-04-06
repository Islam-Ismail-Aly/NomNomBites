using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        public Customer GetByUserId(string userId);
        public IEnumerable<Order> GetOrdersByCustomerId(int customerId);
    }
}
