using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodAndRecipesAPI.Models
{
    public class Food
    {
        public int FoodItemId { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public IFormFile Image { get; set; }
    }
}
