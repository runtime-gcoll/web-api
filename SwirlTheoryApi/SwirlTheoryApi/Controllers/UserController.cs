using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace SwirlTheoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected ShoppingContext context;
        protected UserManager<User> userManager;
        protected SignInManager<User> signInManager;

        public UserController(ShoppingContext _context, UserManager<User> _userManager, SignInManager<User> _signInManager) {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> CreateUserAsync() {
            var result = await userManager.CreateAsync(new WebApi.Models.User {
                UserName = "runtime",
                Email = "runtime@memeware.net"
            }, "Password1."); // Passwords should never hit the memory, they should just be passed down until they can be hashed and stored

            if (result.Succeeded) {
                return Content("User was created", "text/html");
            }
            else {
                return Content("User creation failed", "text/html");
            }
        }

        [Authorize]
        [Route("private")]
        public IActionResult Private() {
            return Content($"This is a private area. Welcome {HttpContext.User.Identity.Name}", "text/html");
        }
    }
}
