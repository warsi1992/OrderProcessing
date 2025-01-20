using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Data;
using OrderProcessingSystem.Model;
using Serilog;

namespace OrderProcessingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly OrderProcessingDbContext _context;

        public ProductController(OrderProcessingDbContext context)
        {
            _context = context;
        }

        // Retrieve all products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // Retrieve a specific product by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                    return NotFound("Product not found.");

                return product;
            }
            catch (Exception ex)
            {
                Log.Error($"Error While getting product details : {ex.Message}");
                return BadRequest(ex.Message);
               
            }
           
        }

        // Add a new product
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(product.Name))
                    return BadRequest("Product Name is required.");

                if (product.Price <= 0)
                    return BadRequest("Price must be greater than zero.");

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                Log.Information("New Product Added Succesfully");
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                Log.Error($"Error while adding new product : {ex.Message}");
                return BadRequest( ex.Message);
            }
           
        }

        // Update an existing product
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            try
            {
                if (id != updatedProduct.Id)
                {
                    Log.Error($"Product id is not valid : {updatedProduct.Id}");
                    return BadRequest("Product ID mismatch.");
                }
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    Log.Error($"Product is not found for id : {id}");
                    return NotFound("Product not found.");
                }
                product.Name = updatedProduct.Name;
                product.Price = updatedProduct.Price;

                await _context.SaveChangesAsync();
                Log.Information("Product updated sucessfully");
                return NoContent();
            }
            catch (Exception ex)
            {

                Log.Error("Error while updating product");
                return BadRequest(ex.Message);
            }
            
        }

        // Delete a product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    Log.Error($"No product is found for id : {id}");
                    return NotFound("Product not found.");
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                Log.Information("Product Deleted");
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error($"Error while deleting the product : {ex.Message}");
                return BadRequest(ex.Message);
            }
            
        }
    }
}
