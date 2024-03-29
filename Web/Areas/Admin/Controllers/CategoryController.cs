using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork<Category> _unitOfWorkCategory;

        public CategoryController(IUnitOfWork<Category> unitOfWorkCategory)
        {
            _unitOfWorkCategory = unitOfWorkCategory;
        }

        [HttpGet]
        public IActionResult Category()
        {
            var categories = _unitOfWorkCategory.Entity.GetAll();
            var categoryViewModels = categories.Select(category => new CategoryViewModel
            {
                CategoryId = category.Id,
                CategoryTitle = category.Title,
                IsAvailable = category.IsAvailable
            });

            return View(categoryViewModels);
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryViewModel viewModel)
        {
            var item = new Category
            {
                Title = viewModel.CategoryTitle,
                IsAvailable = viewModel.IsAvailable
            };

            _unitOfWorkCategory.Entity.Insert(item);
            _unitOfWorkCategory.Save();

            return RedirectToAction(nameof(CategoryController.Category), nameof(Category));
        }

        public IActionResult DeleteCategory(int id)
        {
            _unitOfWorkCategory.Entity.Delete(id);
            _unitOfWorkCategory.Save();

            return RedirectToAction(nameof(CategoryController.Category), nameof(Category));
        }

        public IActionResult EditCategory(CategoryViewModel viewModel)
        {
            var existingCategory = _unitOfWorkCategory.Entity.GetById(viewModel.CategoryId);

            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Title = viewModel.CategoryTitle;
            existingCategory.IsAvailable = viewModel.IsAvailable;

            _unitOfWorkCategory.Entity.Update(existingCategory);
            _unitOfWorkCategory.Save();

            return RedirectToAction(nameof(CategoryController.Category), nameof(Category));
        }
    }
}
