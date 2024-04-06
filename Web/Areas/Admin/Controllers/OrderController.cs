using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork<Customer> _unitOfWorkCustomer;
        private readonly IUnitOfWork<Order> _unitOfWorkOrder;

        public OrderController(IUnitOfWork<Order> unitOfWorkOrder, IUnitOfWork<Customer> unitOfWorkCustomer)
        {
            _unitOfWorkCustomer = unitOfWorkCustomer;
            _unitOfWorkOrder = unitOfWorkOrder;
        }

        public IActionResult Orders(OrderViewModel viewModel)
        {
            ViewBag.Customers = _unitOfWorkCustomer.Entity.GetAll().ToList();

            var orderEntities = _unitOfWorkOrder.Entity.GetAll().ToList();

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
