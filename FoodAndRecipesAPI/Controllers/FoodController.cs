using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodAndRecipesAPI.Models;

namespace FoodAndRecipesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly FoodAndRecipesAPIContext _context;

        public FoodController(FoodAndRecipesAPIContext context)
        {
            _context = context;
        }

        // GET: api/Food
        [HttpGet]
        public IEnumerable<Ingredients> GetFoodItems()
        {

            //return _context.FoodItems;
            return _context.Ingredients;
        }

        // GET: api/Food/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodItems([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foodItems = await _context.FoodItems.FindAsync(id);

            if (foodItems == null)
            {
                return NotFound();
            }

            return Ok(foodItems);
        }

        // PUT: api/Food/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodItems([FromRoute] int id, [FromBody] FoodItems foodItems)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != foodItems.Id)
            {
                return BadRequest();
            }

            _context.Entry(foodItems).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodItemsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Food
        [HttpPost]
        public async Task<IActionResult> PostFoodItems([FromBody] FoodItems foodItems)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FoodItems.Add(foodItems);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFoodItems", new { id = foodItems.Id }, foodItems);
        }

        // DELETE: api/Food/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodItems([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foodItems = await _context.FoodItems.FindAsync(id);
            if (foodItems == null)
            {
                return NotFound();
            }

            _context.FoodItems.Remove(foodItems);
            await _context.SaveChangesAsync();

            return Ok(foodItems);
        }

        private bool FoodItemsExists(int id)
        {
            return _context.FoodItems.Any(e => e.Id == id);
        }
    }
}