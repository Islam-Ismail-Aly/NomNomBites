using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{

	public class CustomerFoodsRepository : ICustomerFoodsRepository
	{
		private readonly ApplicationDbContext _applicationDbContext;
		private readonly UserManager<ApplicationUser> _userManager;

		public CustomerFoodsRepository(ApplicationDbContext applicationDbContext , UserManager<ApplicationUser> userManager)
		{
			_applicationDbContext = applicationDbContext;
			_userManager = userManager;
		}
		public async Task<int> GetUserId( ClaimsPrincipal User)
		{
			var UserId= await _userManager.GetUserAsync(User);
			var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(c => c.ApplicationUserId == UserId.Id);
			return customer.Id;
		}
		public List<CustomerFoods> GetFoodsByCustomerId(int id)
		{
			List<CustomerFoods> mylist = _applicationDbContext.CustomerFoods.Where(c => c.CustomerId == id).ToList();
			return mylist;
		}


		public CustomerFoods GetFoodByCustomerIdAndFoodId(int customer_id, int food_id)
		{

			//int customer_id= _applicationDbContext.Customers.FirstOrDefault(c => c.ApplicationUserId == user_id).Id;
			return _applicationDbContext.CustomerFoods.FirstOrDefault(c => c.CustomerId == customer_id && c.FoodId == food_id);
		}

		public void UpdateFoodByCustomerIdAndFoodId(int customer_id, int food_id, int Quantity, Decimal TotalPrice)
		{
			_applicationDbContext.CustomerFoods.FirstOrDefault(c => c.CustomerId == customer_id && c.FoodId == food_id).Quantity = Quantity;
			_applicationDbContext.CustomerFoods.FirstOrDefault(c => c.CustomerId == customer_id && c.FoodId == food_id).TotalPrice = TotalPrice;

		}
		public void DeleteCustomerFood (CustomerFoods food) 
		{
			_applicationDbContext.Remove(food);
		}

		public void SaveChanges ()
		{
			_applicationDbContext.SaveChanges();
		}
	}
}
