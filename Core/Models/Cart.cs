using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
