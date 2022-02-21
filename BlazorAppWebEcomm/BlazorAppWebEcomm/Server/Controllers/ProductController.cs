using BlazorAppWebEcomm.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWebEcomm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ECommDataBaseContext _context;

        public ProductController(ECommDataBaseContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception)
            {

                return NoContent();
            }
            
        }
    }
}
