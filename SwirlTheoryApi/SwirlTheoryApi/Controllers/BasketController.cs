using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SwirlTheoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    // This whole controller and all of its endpoints need to be locked behind the login gate
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class BasketController : ControllerBase {
    	private readonly ISwirlRepository _repository;

    	public ProductController(ISwirlRepository repository) {
    		_repository = repository;
    	}

    	[HttpGet]
    	[Route("mybasket")]
    	public IActionResult GetMyBasketProducts() {
    		try {
    			string uid = _repository.GetUserIdFromUsername(User.Identity.Name);
    			return Ok(_repository.GetBasketProductsByUserId(uid));
			} catch (Exception ex) {
				return BadRequest(ex);
			}
    	}

    	[HttpPost]
    	[Route("addtobasket")]
    	public IActionResult AddProductToBasket([FromBody] int productId) {
    		// Get the User's ID
    		string uid = _repository.GetUserIdFromUsername(User.Identity.Name);
    		// Get the user object
    		User user = _ctx.Users
                    .Where(u => u.Id == userId)
                    .FirstOrDefault();
            // Figure out which product we're adding
            Product product = _repository.GetProductById(productId);
            // Create a new BasketRow
            BasketRow brow = new BasketRow() {
            	User = user,
            	
            };
    	}
    }
}