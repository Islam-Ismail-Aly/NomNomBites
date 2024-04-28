using Core.Models;

namespace Web.ViewModels
{
    public class FoodDetailsVM
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsAvailable { get; set; }
        public bool FoodInCart {  get; set; }
        public List<Food>? Foods { get; set; } = new List<Food>();

    }
}
