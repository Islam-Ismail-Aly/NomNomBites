using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FoodController : Controller
    {
        private readonly IUnitOfWork<Food> _unitOfWorkFood;
        private readonly IUnitOfWork<Category> _unitOfWorkCategory;

        public FoodController(IUnitOfWork<Food> unitOfWorkFood, IUnitOfWork<Category> unitOfWorkCategory)
        {
            _unitOfWorkFood = unitOfWorkFood;
            _unitOfWorkCategory = unitOfWorkCategory;
        }

        [HttpGet]
        public IActionResult Food(string categoryFilter = null)
        {
            var foods = _unitOfWorkFood.Entity.GetAllIncluding(food => food.Category);

            if (!string.IsNullOrEmpty(categoryFilter))
            {
                foods = foods.Where(food => food.Category.Title == categoryFilter);
            }

            var foodViewModels = foods.Select(food => new FoodViewModel
            {
                Id = food.Id,
                Title = food.Title,
                Description = food.Description,
                Price = food.Price,
                Rating = food.Rating,
                Image = food.Image,
                IsAvailable = food.IsAvailable,
                CategoryId = food.CategoryId,
                CategoryTitle = food.Category.Title
            }).ToList();

            ViewBag.Categories = _unitOfWorkCategory.Entity.GetAll().ToList();

            return View(foodViewModels);
        }


        public IActionResult DeleteFood(int id)
        {
            _unitOfWorkFood.Entity.Delete(id);
            _unitOfWorkFood.Save();

            return RedirectToAction(nameof(FoodController.Food), nameof(Food));
        }
    }
}
