using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class VwUsers
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime CreationDate { get; set; }
        public string Role { get; set; }
    }
}
