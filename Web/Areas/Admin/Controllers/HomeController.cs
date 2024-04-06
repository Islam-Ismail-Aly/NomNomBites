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
        private readonly IUnitOfWork<Customer> _unitOfWorkCustomer;

        public HomeController(UserManager<ApplicationUser> userManager,
            VisitorTracker visitorTracker, IUnitOfWork<Order> unitOfWorkOrder,
            IUnitOfWork<Food> unitOfWorkFood, IUnitOfWork<Category> unitOfWorkCategory, IUnitOfWork<Customer> unitOfWorkCustomer)
        {
            _userManager = userManager;
            _visitorTracker = visitorTracker;
            _unitOfWorkOrder = unitOfWorkOrder;
            _unitOfWorkFood = unitOfWorkFood;
            _unitOfWorkCategory = unitOfWorkCategory;
            _unitOfWorkCustomer = unitOfWorkCustomer;
        }
        public async Task<IActionResult> Index(OrderViewModel viewModel)
        {
            _visitorTracker.TrackUniqueVisitor();

            ViewBag.UniqueVisitorCount = _visitorTracker.GetUniqueVisitorCount();

            ViewBag.UserCount = await _userManager.Users.CountAsync();

            ViewBag.Orders = _unitOfWorkOrder.Entity.GetAll().Count();

            ViewBag.Foods = _unitOfWorkFood.Entity.GetAll().Count();

            var orderEntities = _unitOfWorkOrder.Entity.GetAll().OrderByDescending(o=>o.CreationDate).Take(10).ToList();

            var orderViewModels = orderEntities.Select(order => new OrderViewModel
            {
                Id = order.Id,
                Title = order.Title,
                Price = order.Price,
                TotalPrice = order.Price * order.Quantity,
                Quantity = order.Quantity,
                Status = order.Status,
                CreationDate = order.CreationDate,
                CustomerName = _unitOfWorkCustomer.Entity.GetById(order.CustomerId)?.Name
            }).ToList();

            return View(orderViewModels);
        }


    }
}
