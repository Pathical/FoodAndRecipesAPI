using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodAndRecipesAPI.Models
{
    public class FoodImageItem
    {
        public string Name { get; set; }
        public string Tags { get; set; }
        public IFormFile Image { get; set; }
    }
}
