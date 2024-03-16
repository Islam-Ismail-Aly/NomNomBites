using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public bool Status { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
