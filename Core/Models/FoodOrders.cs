using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class FoodOrders
    {
        [ForeignKey("Foods")]
        public int FoodId { get; set; }
        public virtual ICollection<Food> Foods { get; set; }

        [ForeignKey("Orders")]
        public int OrderId { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
