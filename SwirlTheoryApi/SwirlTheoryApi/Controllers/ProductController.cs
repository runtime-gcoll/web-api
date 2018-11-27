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

        public ProductController(ISwirlRepository repository) {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get() {
            var results = repository.GetAllProducts();
            return Ok(results);
        }
    }
}
