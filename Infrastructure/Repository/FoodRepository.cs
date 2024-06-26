﻿using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
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

        public FoodRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public List<Food> GetOtherTopRatedFoods(int CategoryId)
        {
            var otherFoods = Context.Foods
                                    .Where(f => f.CategoryId != CategoryId)
                                    .OrderBy(f => f.CategoryId) // Ensure ordering before grouping
                                    .ThenBy(f => f.Id)          // Secondary ordering to ensure consistency
                                    .GroupBy(f => f.CategoryId)
                                    .Select(group => group.OrderByDescending(f => f.Rating).First())
                                    .ToList();
            return otherFoods;
        }
        public string GetFoodCategoryName(int CategoryId)
        {
            var foodCategoryName = Context.Categories.Where(c => c.Id == CategoryId).FirstOrDefault().Title;
            return foodCategoryName;
        }

        public List<Food> GetFoodByFoodId(int id)
        {
            var food = Context.Foods.Where(fo => fo.Id == id).FirstOrDefault();
            var foods = Context.Foods
            .Where(f => (f.CategoryId ==food.CategoryId && f.Id!=food.Id) )
            .ToList();

            return foods;
        }
    }
}
