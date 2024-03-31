using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork<Food> _unitOfWorkFood;
        private readonly IUnitOfWork<Category> _unitOfWorkCategory;

        public OrderController(IUnitOfWork<Food> unitOfWorkFood, IUnitOfWork<Category> unitOfWorkCategory)
        {
            _unitOfWorkFood = unitOfWorkFood;
            _unitOfWorkCategory = unitOfWorkCategory;
        }

        public IActionResult Orders()
        {
            return View();
        }
    }
}
