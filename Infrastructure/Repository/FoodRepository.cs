using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class FoodRepository : GenericRepository<Food>, IFoodRepository
    {
        ApplicationDbContext Context;

        public FoodRepository(ApplicationDbContext context):base(context)
        {
            Context = context;
        }

        public List<Food> GetOtherFoods(int CategoryId)
        {
            var otherFoods = Context.Foods
       .Where(f => f.CategoryId != CategoryId)
       .OrderBy(f => f.CategoryId) // Ensure ordering before grouping
       .ThenBy(f => f.Id)          // Secondary ordering to ensure consistency
       .GroupBy(f => f.CategoryId)
       .Select(group => group.First())
       .ToList();
            return otherFoods;
        }
    }
}
