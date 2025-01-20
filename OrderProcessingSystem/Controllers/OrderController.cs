using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Data;
using OrderProcessingSystem.Helper;
using OrderProcessingSystem.Model;
using Serilog;

namespace OrderProcessingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderProcessingDbContext _context;

        public OrderController(OrderProcessingDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            try
            {
                //var customer = await _context.Customers.FindAsync(request.CustomerId);
                var customer = await _context.Customers
                 .Include(c => c.Orders)
                 .ThenInclude(o => o.Products)  // Include related products
                 .FirstOrDefaultAsync(c => c.Id == request.CustomerId);
                if (customer == null)
                    return BadRequest("Invalid customer ID.");

                if (customer.Orders.Any(o => !o.IsFulfilled))
                    return BadRequest("Cannot place a new order until the previous one is fulfilled.");

                var products = await _context.Products
                                             .Where(p => request.ProductIds.Contains(p.Id))
                                             .ToListAsync();

                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    Products = products,
                    OrderDate = DateTime.UtcNow,
                    IsFulfilled = false
                };

                _context.Orders.Add(order);
                _context.Customers.Attach(customer);
                customer.Orders.Add(order);
                await _context.SaveChangesAsync();
                Log.Information($" New order created for customer : {request.CustomerId}");
                return Ok(order);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            try
            {
                var order = await _context.Orders.Include(o => o.Products)
                                             .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                    return NotFound("Order not found.");

                return order;
            }
            catch (Exception ex)
            {

                Log.Error($"Error while getting order");
                return BadRequest(ex.Message);
            }
            
        }
    }
}
