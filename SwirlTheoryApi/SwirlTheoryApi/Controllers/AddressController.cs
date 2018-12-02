using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SwirlTheoryApi.Data;
using SwirlTheoryApi.Data.Entities;

namespace SwirlTheoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class AddressController : Controller {
        private readonly ISwirlRepository _repository;
        private readonly UserManager<User> _userManager;

        public AddressController(ISwirlRepository repository, UserManager<User> userManager) {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("mine")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, Admin")]
        public IActionResult GetMyAddresses() {
            try {
                string uid = _repository.GetUserIdFromUsername(User.Identity.Name);
                return Ok(_repository.GetAddressesByUserId(uid));
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }
    }
}