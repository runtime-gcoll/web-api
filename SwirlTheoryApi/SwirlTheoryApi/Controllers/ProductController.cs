using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwirlTheoryApi.Data;
using SwirlTheoryApi.Data.Entities;

namespace SwirlTheoryApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProductController : Controller {
        private readonly ISwirlRepository _repository;

        public ProductController(ISwirlRepository repository) {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get() {
            List<Product> results = _repository.GetAllProducts().ToList();
            return Ok(results);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product p)
        {
            _repository.AddEntity<Product>(p);
            return Ok(results);
        }
    }
}
