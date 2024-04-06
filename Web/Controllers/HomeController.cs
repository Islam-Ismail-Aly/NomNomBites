using Core.Interfaces;
using Core.Models;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Security.Cryptography;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork<Category> _Categories;
		private readonly IUnitOfWork<Food> _Foods;
		private readonly IUnitOfWork<CustomerFoods> _CustomerFoods;
        private readonly ICustomerFoodsRepository _unitOfWorkCustomerFoods;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork<Category> Categories,
            IUnitOfWork<Food> Foods,
            IUnitOfWork<CustomerFoods> CustomerFoods,
            ICustomerFoodsRepository unitOfWorkCustomerFoods)

        {
            _Foods=Foods;
			_Categories = Categories;
			_logger = logger;
            _CustomerFoods= CustomerFoods;
            _unitOfWorkCustomerFoods = unitOfWorkCustomerFoods;
			
		}

        public IActionResult Index()
        {
            List<Category> categoryList= _Categories.Entity.GetAll().ToList();
            return View("Index", categoryList);
        }

        public List<foodVM>  GetFoodForCategory(int id)
        {
			List<foodVM>  mylist = _Categories.Entity.GetById(id).Foods.Select(f => new foodVM(f.Id, f.Price , f.Title, f.Description , f.Image, f.IsAvailable)).ToList();
            return mylist;
		}

        public List<foodVM> GetFoods()
        {
            List<Food > li = _Foods.Entity.GetAll().ToList();
            List<foodVM> mylist = li.Select(f => new foodVM(f.Id, f.Price, f.Title, f.Description, f.Image, f.IsAvailable)).ToList();
            return mylist;
        }

        public IActionResult AddFoodToCart(int foodId)
        {
			var _CustomerId = _unitOfWorkCustomerFoods.GetUserId(User).Result;
			if (_unitOfWorkCustomerFoods.GetFoodByCustomerIdAndFoodId(_CustomerId, foodId) == null) {
                 CustomerFoods customerFoods = new CustomerFoods { FoodId = foodId, CustomerId = _CustomerId, Quantity = 1, TotalPrice = _Foods.Entity.GetById(foodId).Price };
                _CustomerFoods.Entity.Insert(customerFoods);
                _CustomerFoods.Save();
            }
           return NoContent();
        }



        public IActionResult About()
        {
            return View();
        } 
        
        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
