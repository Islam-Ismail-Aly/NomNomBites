using Core.Models;

namespace Web.ViewModels
{
    public class CustomerOrdersViewModel
    {
        public int CustomerId { get; set;}
        public IEnumerable<Order> CustomerOrders { get; set; }
    }
}
