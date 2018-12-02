using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SwirlTheoryApi.Data;
using SwirlTheoryApi.Data.DTO;
using SwirlTheoryApi.Data.Entities;

namespace SwirlTheoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    // Since we want anyone to be able to sign up for a new account, and obviously you can't need to be logged in
    // in order to log in, only the ElevateToAdmin() method below has it's own Authorize tag
    public class AccountController : Controller {
        private readonly ShoppingContext _ctx;
        private readonly ILogger<AccountController> _logger; // Log errors
        private readonly UserManager<User> _userManager; // For creating new users and changing user roles
        private readonly SignInManager<User> _signInManager; // For logging users in
        private readonly IConfiguration _config; // We need access to JWT info stored in config.json

        public AccountController(ShoppingContext ctx,
            ILogger<AccountController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration config,
            IServiceProvider serviceProvider) {
            _ctx = ctx;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> CreateToken([FromBody] LoginDTO model) {
            // If the state matches the validation in the LoginDTO we got passed
            if (ModelState.IsValid) {
                // Try to find a user with the supplied email
                User user = await _userManager.FindByEmailAsync(model.Email);
                // If one exists, check that the password is valid
                if (user != null) {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    // If the password *is* valid, then create a JWT and kick it back to the client
                    if (result.Succeeded) {
                        // Define the JWT's claims
                        var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email), // The user's email address
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // A unique string used to ID the token
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName), // The user's username
                        };

                        // Key for encryption (comes from config.json)
                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        // Create the token
                        JwtSecurityToken token = new JwtSecurityToken(
                            // These are both specified in config.json
                            _config["Tokens:Issuer"], // Where does the token come from
                            _config["Tokens:Audience"], // Who is the token supposed to be used by
                            claims, // What claims does the token contain
                            expires: DateTime.UtcNow.AddMinutes(30), // When does the token expire
                            signingCredentials: creds // What are the signing credentials (private key)
                            );

                        // Create an anonymous object with the token as a string and the expiry date
                        // This gets spat out as JSON data at the other end, which the client can easily parse
                        object results = new {
                            token = new JwtSecurityTokenHandler().WriteToken(token), // Returns an actual string
                            expiration = token.ValidTo // The date and time at which the token expires
                        };

                        // Return empty string because there's no source for the object
                        // Then return the results object (containing the key and the expiry date)
                        return Created("", results);
                    }
                }
            }

            // If anything above fails to validate (wrong email, wrong password, etc.) then just return a Bad Request
            return BadRequest("Login failed");
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO model) {
            if (ModelState.IsValid) {
                // We don't need to do email checking because we have UniqueEmails on

                // If the passwords entered are the same
                if (model.Password == model.ConfirmPassword) {
                    // Make a new User object
                    User user = new User() {
                        Email = model.Email,
                        UserName = model.Email
                    };

                    await _userManager.CreateAsync(user, model.Password);
                    return Created("", user);
                }
            }

            return BadRequest("Registration failed");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Only an Admin account can make another account an Admin
        [Route("elevate")]
        public async Task<ActionResult> ElevateToAdmin(string userId) {
            User user = _ctx.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault();

            await _userManager.AddToRoleAsync(user, "Admin");

            return BadRequest();
        }
    }
}