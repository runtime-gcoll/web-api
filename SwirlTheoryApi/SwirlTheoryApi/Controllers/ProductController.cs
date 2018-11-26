using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace SwirlTheoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller {
        private readonly ShoppingContext _context;

        public ProductController(ShoppingContext context) {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get() {
            var results = _context.Products.ToList();
            return Ok(results);
        }
    }
}