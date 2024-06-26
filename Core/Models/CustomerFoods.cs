﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CustomerFoods
    {
        [ForeignKey("Food")]
        public int FoodId { get; set; }
        public virtual Food Food { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
