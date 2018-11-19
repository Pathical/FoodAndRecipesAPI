using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FoodAndRecipesAPI.Models
{
    public class FoodAndRecipesAPIContext : DbContext
    {
        public FoodAndRecipesAPIContext (DbContextOptions<FoodAndRecipesAPIContext> options)
            : base(options)
        {
        }

        public DbSet<FoodAndRecipesAPI.Models.FoodItems> FoodItems { get; set; }
        public DbSet<FoodAndRecipesAPI.Models.Ingredients> Ingredients { get; set; }
    }
}
