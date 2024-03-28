using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Areas.Admin.ViewModels;
using Web.Utilities;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : AdminBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VisitorTracker _visitorTracker;
        private readonly IUnitOfWork<Order> _unitOfWorkOrder;
        private readonly IUnitOfWork<Food> _unitOfWorkFood;
        private readonly IUnitOfWork<Category> _unitOfWorkCategory;

        public HomeController(UserManager<ApplicationUser> userManager, 
            VisitorTracker visitorTracker, IUnitOfWork<Order> unitOfWorkOrder, 
            IUnitOfWork<Food> unitOfWorkFood, IUnitOfWork<Category> unitOfWorkCategory)
        {
            _userManager = userManager;
            _visitorTracker = visitorTracker;
            _unitOfWorkOrder = unitOfWorkOrder;
            _unitOfWorkFood = unitOfWorkFood;
            _unitOfWorkCategory = unitOfWorkCategory;
        }
        public async Task<IActionResult> Index()
        {
            _visitorTracker.TrackUniqueVisitor();

            ViewBag.UniqueVisitorCount = _visitorTracker.GetUniqueVisitorCount();

            ViewBag.UserCount = await _userManager.Users.CountAsync();

            ViewBag.Orders = _unitOfWorkOrder.Entity.GetAll().Count();

            ViewBag.Foods = _unitOfWorkFood.Entity.GetAll().Count();

            return View();
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
    }
}
