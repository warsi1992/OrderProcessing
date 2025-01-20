using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderProcessingSystem.Data;
using OrderProcessingSystem.Model;
using Serilog;
using System.Xml;

namespace OrderProcessingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        
        private readonly OrderProcessingDbContext _context;

        public CustomerController(OrderProcessingDbContext context)
        {
            _context = context;
        
        }

        // Add a new customer
        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {

            // Validate input
            try
            {
                if (string.IsNullOrWhiteSpace(customer.Name) || string.IsNullOrWhiteSpace(customer.Email))
                    return BadRequest("Name and Email are required.");

                // Add the new customer to the database
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                Log.Information("Customer Added Successfully");
                // Return the created customer
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                Log.Error($"Error while adding Customer : {ex.Message}");
                return BadRequest(ex.Message);
                
            }

        }

        // Retrieve a specific customer
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                Log.Information("Fetching customer with ID: {CustomerId}", id);
                // _logger.LogInformation("Fetching customer with ID: {CustomerId}", id);
                // var customer = await _context.Customers.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Id == id);
                var customer = _context.Customers
                 .Include(c => c.Orders)
                 .ThenInclude(o => o.Products)
                 .FirstOrDefault(c => c.Id == id);
                if (customer == null)
                {
                    Log.Information("Invalid customer ID: {CustomerId}", id);

                    // _logger.LogWarning("Invalid customer ID: {CustomerId}", id);
                    return NotFound("Customer not found.");
                }
                string customerJson = JsonConvert.SerializeObject(customer, Newtonsoft.Json.Formatting.Indented,
         new JsonSerializerSettings
         {
             ReferenceLoopHandling = ReferenceLoopHandling.Ignore
         });

                Log.Information("Customer Data: {CustomerJson}", customerJson);
                return customer;
            }
            catch (Exception ex)
            {

                Log.Error($"Exception while getting CUstomer Record : {ex.Message}");
                return BadRequest(ex.Message);
            }
            
            //Log.Information(customer.ToString());
            
        }
    }
}
