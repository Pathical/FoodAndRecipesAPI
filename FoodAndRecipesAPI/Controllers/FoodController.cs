using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodAndRecipesAPI.Models;
using FoodAndRecipesAPI.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.Extensions.Configuration;

namespace FoodAndRecipesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly FoodAndRecipesAPIContext _context;
        private IConfiguration _configuration;

        public FoodController(FoodAndRecipesAPIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Food
        [HttpGet]
        public IEnumerable<FoodItems> GetFoodItems()
        {
            return _context.FoodItems;
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

        // GET: api/Food/Tags
        [Route("tags")]
        [HttpGet]
        public async Task<List<string>> GetTags()
        {
            var memes = (from m in _context.FoodItems
                         select m.Tags).Distinct();

            var returned = await memes.ToListAsync();

            return returned;
        }

        [HttpPost, Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm]FoodImageItem food)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }
            try
            {
                using (var stream = food.Image.OpenReadStream())
                {
                    var cloudBlock = await UploadToBlob(food.Image.FileName, null, stream);
                    //// Retrieve the filename of the file you have uploaded
                    //var filename = provider.FileData.FirstOrDefault()?.LocalFileName;
                    if (string.IsNullOrEmpty(cloudBlock.StorageUri.ToString()))
                    {
                        return BadRequest("An error has occured while uploading your file. Please try again.");
                    }

                    FoodItems foodItems = new FoodItems();
                    foodItems.Name = food.Name;
                    foodItems.Tags = food.Tags;

                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    foodItems.Height = image.Height.ToString();
                    foodItems.Width = image.Width.ToString();
                    foodItems.Url = cloudBlock.SnapshotQualifiedUri.AbsoluteUri;
                    foodItems.Uploaded = DateTime.Now.ToString();

                    _context.FoodItems.Add(foodItems);
                    await _context.SaveChangesAsync();

                    return Ok($"File: {food.Name} has successfully uploaded");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }


        }

        private async Task<CloudBlockBlob> UploadToBlob(string filename, byte[] imageBuffer = null, System.IO.Stream stream = null)
        {

            var accountName = _configuration["AzureBlob:name"];
            var accountKey = _configuration["AzureBlob:key"]; ;
            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer imagesContainer = blobClient.GetContainerReference("images");

            string storageConnectionString = _configuration["AzureBlob:connectionString"];

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Generate a new filename for every new blob
                    var fileName = Guid.NewGuid().ToString();
                    fileName += GetFileExtention(filename);

                    // Get a reference to the blob address, then upload the file to the blob.
                    CloudBlockBlob cloudBlockBlob = imagesContainer.GetBlockBlobReference(fileName);

                    if (stream != null)
                    {
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    }
                    else
                    {
                        return new CloudBlockBlob(new Uri(""));
                    }

                    return cloudBlockBlob;
                }
                catch (StorageException ex)
                {
                    return new CloudBlockBlob(new Uri(""));
                }
            }
            else
            {
                return new CloudBlockBlob(new Uri(""));
            }

        }

        private string GetFileExtention(string fileName)
        {
            if (!fileName.Contains("."))
                return ""; //no extension
            else
            {
                var extentionList = fileName.Split('.');
                return "." + extentionList.Last(); //assumes last item is the extension 
            }
        }
    }
}