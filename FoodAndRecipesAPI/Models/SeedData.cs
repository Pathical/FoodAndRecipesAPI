﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodAndRecipesAPI.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FoodAndRecipesAPIContext(
                serviceProvider.GetRequiredService<DbContextOptions<FoodAndRecipesAPIContext>>()))
            {
                // Look for any movies.
                if (context.FoodItems.Count() > 0)
                {
                    return;   // DB has been seeded
                }

                context.FoodItems.AddRange(
                    new FoodItems
                    {
                        Name = "Chocolate Cake",
                        Url = "http://www.meencantaelcafe.com/wp-content/uploads/2016/01/receta-preparar-brownie-cafe-324x160.jpg",
                        Tags = "Cake",
                        Uploaded = "07-10-18 4:20T18:25:43.511Z",
                        Description = "Delicious chocolate cake that everyone will enjoy!",
                        IngredientsList = new List<Ingredients>
                        {
                            new Ingredients { Ingredient = "1 banana"},
                            new Ingredients { Ingredient = "1cup of apple"}
                        },
                        Instructions ="Preheat oven to 180 degrees. Add flour, cocoa, baking powder and white sugar into a bowl...  ",
                        Width = "324",
                        Height = "160"
                    }
                );

                context.FoodItems.AddRange(
                    new FoodItems
                    {
                        Name = "testData",
                        Url = "http://www.meencantaelcafe.com/wp-content/uploads/2016/01/receta-preparar-brownie-cafe-324x160.jpg",
                        Tags = "Testing",
                        Uploaded = "07-10-18 4:20T18:25:43.511Z",
                        Description = "This is test data for initializing the database",
                        IngredientsList = new List<Ingredients>
                        {
                            new Ingredients { Ingredient = "2 drops of Unicorn tears"},
                            new Ingredients { Ingredient = "1 tbsp of Love"},
                            new Ingredients { Ingredient = "3 tsp of  Bleach"}
                        },
                        Instructions = "There are no instructions here.",
                        Width = "324",
                        Height = "160"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
