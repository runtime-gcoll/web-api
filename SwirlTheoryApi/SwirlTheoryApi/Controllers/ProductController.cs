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
    [ApiController]
    public class ProductController : Controller {
        private readonly ISwirlRepository _repository;

        public ProductController(ISwirlRepository repository) {
            _repository = repository;
        }

        // Get products, can be accessed by anybody
        [HttpGet]
        [Route("all")]
        public IActionResult Get() {
            try
            {
                List<Product> results = _repository.GetAllProducts().ToList();
                return Ok(results);
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        // Search can be accessed by anybody
        [HttpGet]
        [Route("search")]
        public IActionResult SearchProducts(string searchTerm) {
            try {
                List<Product> results = _repository.SearchProducts(searchTerm).ToList();
                return Ok(results);
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        // Creating products, can only be done by Admins
        [HttpPost]
        [Route("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult Post([FromBody] Product p)
        {
            try {
                _repository.AddEntity(p);
                _repository.SaveAll();
                // We can get p.ProductId here even though it's never specified by the user, becuase
                // once you call _repository.SaveAll(), the data in the database gets passed back to
                // the model in this function
                return Created($"/api/products/{p.ProductId}", p);
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        // Updating products, only for Admins
        [HttpPut]
        [Route("update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult Put([FromBody] Product p) {
            try {
                _repository.UpdateProduct(p);
                _repository.SaveAll();

                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        // Deleting products, only for Admins
        [HttpDelete]
        [Route("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult Delete(int productId) {
            try {
                _repository.DeleteProduct(productId);
                _repository.SaveAll();
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }
    }
}
