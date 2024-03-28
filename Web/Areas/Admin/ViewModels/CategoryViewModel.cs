using Core.Models;

namespace Web.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public bool IsAvailable { get; set; }
        //public ICollection<Food> Foods { get; set; }
    }

    //public class Food
    //{
    //    public int FoodId { get; set; }
    //    public string FoodTitle { get; set; }
    //    public decimal Price { get; set; }
    //    public byte[] Image { get; set; }
    //}
}
