using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SwirlTheoryApi.Data.DTO;
using SwirlTheoryApi.Data.Entities;

namespace SwirlTheoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config; // We need access to JWT info stored in config.json

        public AccountController(ILogger<AccountController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration config) {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost]
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
                        // Then return
                        return Created("", results);
                    }
                }
            }

            // If anything above fails to validate (wrong email, wrong password, etc.) then just return a Bad Request
            return BadRequest();
        }
    }
}