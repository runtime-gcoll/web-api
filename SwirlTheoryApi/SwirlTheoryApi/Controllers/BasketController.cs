using System;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwirlTheoryApi.Data;
using SwirlTheoryApi.Data.Entities;

namespace SwirlTheoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    // This whole controller and all of its endpoints need to be locked behind the login gate
    // Since we're only changing user-specific data right now, nothing needs to be AdminOnly
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BasketController : ControllerBase {
    	private readonly ISwirlRepository _repository;
        private readonly ShoppingContext _ctx;

    	public BasketController(ISwirlRepository repository, ShoppingContext ctx) {
    		_repository = repository;
            _ctx = ctx;
    	}

    	[HttpGet]
    	[Route("mybasket")]
    	public IActionResult GetMyBasketProducts() {
    		try {
    			string uid = _repository.GetUserIdFromUsername(User.Identity.Name);
    			return Ok(_repository.GetBasketRowsByUserId(uid));
			} catch (Exception ex) {
				return BadRequest(ex);
			}
    	}

    	[HttpPost]
    	[Route("addtobasket")]
    	public IActionResult AddProductToBasket(int productId, int quantity = 1) {
    		// Get the User's ID
    		string uid = _repository.GetUserIdFromUsername(User.Identity.Name);
            // Get the user object
            User user = _repository.GetUserByUserId(uid);
            // Figure out which product we're adding
            Product product = _repository.GetProductById(productId);

            // Don't allow a user to add more of a product to their basket than we have in stock
            if (quantity > product.Quantity)
            {
                return BadRequest("Not enough items in stock");
            }
            else {
                // Create a new BasketRow
                BasketRow brow = new BasketRow()
                {
                    User = user,
                    Product = product,
                    Quantity = quantity // Qty is set to 1 by default
                };

                // Change the product info to reflect the reducement in stock
                product.Quantity -= quantity;
                _repository.UpdateProduct(product);

                // Add the BasketRow to the database
                _repository.AddEntity(brow);
                // Save changes
                _repository.SaveAll();

                return Created("", brow);
            }
    	}

        [HttpPut]
        [Route("changequantity")]
        public IActionResult ChangeProductBasketQuantity(int productId, int quantity) {
            try
            {
                // UserId, you know the drill by now
                string uid = _repository.GetUserIdFromUsername(User.Identity.Name);
                // Get the BasketRow we're modifying
                BasketRow brow = _repository.GetBasketRowByUserProduct(uid, productId);
                // Change it
                brow.Quantity = quantity;
                // Commit it
                _repository.UpdateBasketRow(brow);
                // Save it
                _repository.SaveAll();
                return Ok(brow); // Return the new resource
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }
        
        [HttpDelete]
        [Route("remove")]
        public IActionResult RemoveFromBasket(int productId) {
            try {
                string uid = _repository.GetUserIdFromUsername(User.Identity.Name);
                // When we do this for a BasketRow that never goes through for an order, we need to restore the "reserved" quantity in the BasketRow back to the Product
                // so that other people can order it
                BasketRow brow = _repository.GetBasketRowByUserProduct(uid, productId); // Note: Could userId + productId have been a composite PK for the BasketRow table to cut down on storage space???
                Product product = _repository.GetProductById(productId);
                product.Quantity += brow.Quantity;
                _repository.UpdateProduct(product);
                // Now we can safely delete the BasketRow
                _repository.DeleteBasketRow(uid, productId);
                // Always Ctrl+S
                _repository.SaveAll();
                return Ok(brow); // Honestly not sure if returning the deleted resource is standard practice
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }
    }
}