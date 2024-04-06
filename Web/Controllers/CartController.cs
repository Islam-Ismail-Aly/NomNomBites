using Microsoft.AspNetCore.Mvc;
using Core.Models;
using Core.Interfaces;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
namespace Web.Controllers
{
    public class CartController : Controller
    {
		
		private readonly IUnitOfWork<Customer> _unitOfWork;
        private readonly ICustomerFoodsRepository _unitOfWorkCustomerFoods;

		public CartController(IUnitOfWork<Customer> unitOfWork,
			ICustomerFoodsRepository unitOfWorkCustomerFoods)
        {
			this._unitOfWorkCustomerFoods = unitOfWorkCustomerFoods;
			_unitOfWork = unitOfWork;

		}
        public IActionResult Show() {

			var CustomerId = _unitOfWorkCustomerFoods.GetUserId(User).Result;
			return View("Show", _unitOfWorkCustomerFoods.GetFoodsByCustomerId(CustomerId));
        }

        public List<int> FoodsInCustomerCart()
        {
			var CustomerId = _unitOfWorkCustomerFoods.GetUserId(User).Result;
			return _unitOfWorkCustomerFoods.GetFoodsByCustomerId(CustomerId).Select(f=>f.FoodId).ToList();
        }

		public void UpdateCart( int food_id, int Quantity, Decimal TotalPrice)
		{
			var CustomerId = _unitOfWorkCustomerFoods.GetUserId(User).Result;
			_unitOfWorkCustomerFoods.UpdateFoodByCustomerIdAndFoodId(CustomerId, food_id, Quantity, TotalPrice);
			_unitOfWorkCustomerFoods.SaveChanges();
		}


		public IActionResult RemoveItem(int customer_id, int food_id)
		{
			var CustomerId = _unitOfWorkCustomerFoods.GetUserId(User).Result;
			CustomerFoods item = _unitOfWorkCustomerFoods.GetFoodByCustomerIdAndFoodId(CustomerId, food_id);
			_unitOfWorkCustomerFoods.DeleteCustomerFood(item);
			_unitOfWorkCustomerFoods.SaveChanges();
			return NoContent();
		}


		public IActionResult Checkout()
		{
			var CustomerId = _unitOfWorkCustomerFoods.GetUserId(User).Result;
			List< CustomerFoods > CustomerFoodsList = _unitOfWorkCustomerFoods.GetFoodsByCustomerId(CustomerId).ToList();
			ViewBag.Subtotal = CustomerFoodsList.Select(f=>f.TotalPrice).Sum();
			ViewBag.Customer = _unitOfWork.Entity.GetById(CustomerId);

			return View("checkout", CustomerFoodsList);
		}


	}
}
