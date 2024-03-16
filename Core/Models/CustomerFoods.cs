using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CustomerFoods
    {
        public ICollection<Food> Foods { get; set; } = new List<Food>();
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
