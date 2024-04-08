namespace Web.ViewModels
{
    public class FoodSameCategoryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
        public bool IsAvailable { get; set; }
    }
}
