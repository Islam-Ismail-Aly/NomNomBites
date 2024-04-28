using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFoodRepository:IGenericRepository<Food>
    {
        List<Food> GetOtherTopRatedFoods(int CategoryId);
        string GetFoodCategoryName(int CategoryId);
        public List<Food> GetFoodByFoodId(int id);
    }
}
