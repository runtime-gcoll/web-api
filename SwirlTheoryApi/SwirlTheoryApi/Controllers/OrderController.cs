using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
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
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class OrderController : Controller {
        private readonly ISwirlRepository _repository;

        public OrderController(ISwirlRepository repository) {
            _repository = repository;
        }

        // Return all orders, only for admins
        [HttpGet]
        [Route("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
        public IActionResult GetAll() {
            try {
                return Ok(_repository.GetOrders());
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        // NOTE: Implementing the Order access methods separately fixes a minor security issue that existed in the initial
        // implementation. Originally, access was done entirely by the first method, which was accessible to Users and
        // Admins, with the intention that the frontend would only ever request orders that matched the currently
        // logged in User's UserId in the data service for products. This would have meant that if somebody could intercept
        // an HTTP request between the frontend and the backend, and they knew what another User's UserId was, they could
        // have the backend return the list of Orders that belonged to that other User.

        // The way that the methods now work means that only an Admin can request a particular UserId to see the Orders for
        // (which means that an Admin could still perform this "exploit", but there's no point since they're *supposed* to
        // have access to all this data anyway), whereas when a regular user wants to see their own order, the check of
        // the User's UserName then returns their UserId, which gives the client side no opportunity to alter the data
        // before it gets here.

        // Allow Admins to see a list of Orders associated with a user
        [HttpGet]
        [Route("foruser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
        public IActionResult GetForUser(string userId) {
            try {
                return Ok(_repository.GetOrdersByUserId(userId));
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("myorders")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetMyOrders()
        {
            try {
                string uid = _repository.GetUserIdFromUsername(User.Identity.Name);
                return Ok(_repository.GetOrdersByUserId(uid));
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        // Everyone needs to be able to convert their BasketItems into an order
        [HttpGet]
        [Route("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create() {
            try {
                // Get the current user's ID
                string uid = _repository.GetUserIdFromUsername(User.Identity.Name);
                // Get the User to attach to the Order
                User user = _repository.GetUserByUserId(uid);
                // Get all of the user's BasketRows
                List<BasketRow> brows = _repository.GetBasketRowsByUserId(uid).ToList();
                // Create a new Order
                Order newOrder = new Order() {
                    User = user,
                    OrderDate = DateTime.UtcNow,
                    OrderStatus = OrderStatus.Ordered
                };
                // Make a list to store the new OrderRows
                List<OrderRow> newOrderRows = new List<OrderRow>();
                // For each BasketRow, construct an OrderRow item, then add it to the list that's going in the Order
                foreach (BasketRow brow in brows) {
                    OrderRow or = new OrderRow() {
                        Order = newOrder,
                        Product = brow.Product,
                        Quantity = brow.Quantity
                    };

                    newOrderRows.Add(or);
                }
                // Add the OrderRows the Order
                newOrder.OrderRows = newOrderRows;
                // Commit the Order to the repository as a new Entity
                // It should interface with the DB properly and the Order into the Order table
                // And the OrderRows into the OrderRow table with all the foreign keys linked up properly
                _repository.AddEntity(newOrder);
                // NOTE: OrderRows do not get added to the DB when you do this, so add them manually
                foreach (OrderRow or in newOrderRows) {
                    _repository.AddEntity(or);
                }
                // Now we can delete all the BasketRows, since they've been converted to OrderRows and we don't need them anymore
                foreach (BasketRow brow in brows) {
                    _repository.DeleteBasketRow(brow.User.Id, brow.Product.ProductId);
                }
                // And save the changes to the DB
                _repository.SaveAll();
                // Return a 200 and the Order which was created
                return Ok(newOrder);
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }
    }
}