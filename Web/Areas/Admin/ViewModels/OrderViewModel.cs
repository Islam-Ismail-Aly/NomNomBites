namespace Web.Areas.Admin.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public string CustomerName { get; set;}
    }
}
