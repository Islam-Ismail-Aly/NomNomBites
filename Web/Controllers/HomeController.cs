using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
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
        public HomeController(ILogger<HomeController> logger, IUnitOfWork<Food>unitOfWork, IFoodRepository foodRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            this.foodRepository = foodRepository;
        }

        public IActionResult Index()
        {
            var foods = _unitOfWork.Entity.GetAll();

            return View("Index",foods);
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
        public IActionResult Details(int id) {
            var Food = _unitOfWork.Entity.GetById(id);
            var otherFoods = foodRepository.GetOtherFoods(Food.CategoryId);

            var viewModel = new FoodDetailsVM() { 
            Foods=otherFoods,
            Title=Food.Title,
            Description=Food.Description,
            Price=Food.Price,
            Image= Food.Image,
            Rating=Food.Rating,
            IsAvailable=Food.IsAvailable,
            CategoryId=Food.CategoryId
            };
           
            return View("Details",viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
