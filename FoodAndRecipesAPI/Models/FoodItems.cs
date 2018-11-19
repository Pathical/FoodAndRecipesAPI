using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodAndRecipesAPI.Models
{
    public class FoodItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; } // pizza, pasta, cake
        public string Uploaded { get; set; }
        public string Description { get; set; }
        public List<Ingredients> IngredientsList{ get; set; }
        public string Instructions { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    
    }
    public class Ingredients
    {
        public int Id { get; set; }
        public string Ingredient { get; set; }
    }
}
