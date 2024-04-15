using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Diagnostics;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork<Food> _unitOfWork;
        private readonly IFoodRepository foodRepository;
        private readonly IUnitOfWork<Category> _Categories;
        private readonly IUnitOfWork<Food> _Foods;
        private readonly IUnitOfWork<CustomerFoods> _CustomerFoods;
        private readonly ICustomerFoodsRepository _unitOfWorkCustomerFoods;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork<Food> unitOfWork, IFoodRepository foodRepository,
            IUnitOfWork<Category> Categories,
            IUnitOfWork<Food> Foods,
            IUnitOfWork<CustomerFoods> CustomerFoods,
            ICustomerFoodsRepository unitOfWorkCustomerFoods)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            this.foodRepository = foodRepository;
            _Foods = Foods;
            _Categories = Categories;
            _CustomerFoods = CustomerFoods;
            _unitOfWorkCustomerFoods = unitOfWorkCustomerFoods;
        }

        public IActionResult Index()
        {
            List<Category> categoryList = _Categories.Entity.GetAll().ToList();
            return View("Index", categoryList);
        }

        public List<foodVM> GetFoodForCategory(int id)
        {
            List<foodVM> mylist = _Categories.Entity.GetById(id).Foods.Select(f => new foodVM(f.Id, f.Price, f.Title, f.Rating, f.Image, f.IsAvailable)).ToList();
            return mylist;
        }

        public List<foodVM> GetFoods()
        {
            List<Food> li = _Foods.Entity.GetAll().ToList();
            List<foodVM> mylist = li.Select(f => new foodVM(f.Id, f.Price, f.Title, f.Rating, f.Image, f.IsAvailable)).ToList();
            return mylist;
        }

        public IActionResult AddFoodToCart(int foodId)
        {
            var _CustomerId = _unitOfWorkCustomerFoods.GetUserId(User).Result;
            if (_unitOfWorkCustomerFoods.GetFoodByCustomerIdAndFoodId(_CustomerId, foodId) == null)
            {
                CustomerFoods customerFoods = new CustomerFoods { FoodId = foodId, CustomerId = _CustomerId, Quantity = 1, TotalPrice = _Foods.Entity.GetById(foodId).Price };
                _CustomerFoods.Entity.Insert(customerFoods);
                _CustomerFoods.Save();
            }
            return NoContent();
        }

        public IActionResult About()
        {
            return View("About");
        }
        public IActionResult ContactUs()
        {
            return View("ContactUs");
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var Food = _unitOfWork.Entity.GetById(id);
            //var otherFoods = foodRepository.GetOtherTopRatedFoods(Food.CategoryId);
            var _CustomerId = _unitOfWorkCustomerFoods.GetUserId(User).Result;
            var foodInCart = (_unitOfWorkCustomerFoods.GetFoodByCustomerIdAndFoodId(_CustomerId, id) != null) ? true : false;
            var foodCategoryName = _Categories.Entity.GetById(Food.CategoryId).Title;
            var viewModel = new FoodDetailsVM()
            {
                Id = id,
                Title = Food.Title,
                CategoryName = foodCategoryName,
                Description = Food.Description,
                Price = Food.Price,
                Image = Food.Image,
                Rating = Food.Rating,
                FoodInCart = foodInCart,
                IsAvailable = Food.IsAvailable,
                CategoryId = Food.CategoryId
            };

            //ViewBag.Foods = foodRepository.GetOtherTopRatedFoods(Food.CategoryId);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult GetFoodByFoodId(int id)
        {
            var foodByCategoryId = foodRepository.GetFoodByFoodId(id)
                .Select(f => new FoodSameCategoryViewModel
                {
                    Id = f.Id,
                    Title = f.Title,
                    Description = f.Description,
                    Price = f.Price,
                    Image = f.Image,
                    Rating = f.Rating,
                    IsAvailable = f.IsAvailable
                })
            .ToList();

            return Json(foodByCategoryId);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
