using Core.Models;

namespace Web.Areas.Admin.ViewModels
{
    public class FoodViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        //public string Image { get; set; }
        public double Rating { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryTitle { get; set; }
    }
}
