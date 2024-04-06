using Core.Models;

namespace Web.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public bool IsAvailable { get; set; }
    }
}
