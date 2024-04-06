using Core.Models;
using System.Security.Claims;

namespace Infrastructure.Repository
{
	public interface ICustomerFoodsRepository
	{
		public  Task<int> GetUserId(ClaimsPrincipal User);
		public List<CustomerFoods> GetFoodsByCustomerId(int id);
		public CustomerFoods GetFoodByCustomerIdAndFoodId(int customer_id,int food_id);
		public void UpdateFoodByCustomerIdAndFoodId(int customer_id,int food_id,int Quantity,Decimal TotalPrice);
		public void DeleteCustomerFood(CustomerFoods food);
		public void SaveChanges();
	}
}