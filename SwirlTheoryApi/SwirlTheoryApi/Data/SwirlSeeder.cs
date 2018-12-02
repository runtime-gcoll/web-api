using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwirlTheoryApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SwirlTheoryApi.Data
{
    public class SwirlSeeder {
        private readonly ShoppingContext _ctx;
        private readonly UserManager<User> _userManager;
        private readonly IServiceProvider _serviceProvider;

        public SwirlSeeder(ShoppingContext ctx,
            UserManager<User> userManager,
            IServiceProvider serviceProvider) {
            _ctx = ctx;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
        }

        public async Task SeedAsync() {
            // NOTE: Should this be _ctx.Migrate() ?????
            _ctx.Database.EnsureCreated();

            // Create User roles
            // Get a RoleManager
            RoleManager<IdentityRole> roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            // Specify the roles we want to create
            string[] roleNames = { "User", "Admin" };
            // Loop through them
            foreach (var roleName in roleNames) {
                // Check if the role already exists
                bool roleExist = await roleManager.RoleExistsAsync(roleName);
                // If it doesn't, then create it
                if (!roleExist) {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Set up a User
            User adminUser = await _userManager.FindByEmailAsync("runtime@memeware.net");

            // Add a dummy user
            if (adminUser == null) {
                adminUser = new User()
                {
                    ProfileImageUrl = "https://ibb.co/XsW01rm",
                    Email = "runtime@memeware.net",
                    UserName = "runtime@memeware.net",
                };

                // Create the user in the DB
                IdentityResult adminResult = await _userManager.CreateAsync(adminUser, "P@ssw0rd!");
                // If it's not successful, then throw an error
                if (adminResult != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
                // Otherwise, assign the user to the Admin and User roles
                else
                {
                    await _userManager.AddToRoleAsync(adminUser, roleNames[0]);
                    await _userManager.AddToRoleAsync(adminUser, roleNames[1]);
                }
            }

            // Now, set up another user who isn't an Admin
            // Set up a User
            User normalUser = await _userManager.FindByEmailAsync("j.r.user@gmail.com");

            // Add a dummy user
            if (normalUser == null)
            {
                normalUser = new User()
                {
                    ProfileImageUrl = "https://thewondrous.com/wp-content/uploads/2015/07/stylish-profile-pictures-for-facebook-for-girls.jpg",
                    Email = "j.r.user@gmail.com",
                    UserName = "j.r.user@gmail.com",
                };

                // Create the user
                var normalResult = await _userManager.CreateAsync(normalUser, "P@ssw0rd!");
                // If it's not successful, then throw an error
                if (normalResult != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
                // Otherwise, assign the user to the Admin role
                else
                {
                    await _userManager.AddToRoleAsync(normalUser, roleNames[0]);
                }
            }

            // Add default product data
            if (!_ctx.Products.Any()) {
                // Add four default products
                _ctx.Products.Add(new Product {
                    ProductTitle = "Carolina Avengers",
                    ProductDescription = "A totally WIZARD shirt for the sports nerd in your life!",
                    ImageUrl = "https://i.redd.it/ogmlb210je021.jpg",
                    Cost = 35.00F,
                    Quantity = 100
                });

                _ctx.Products.Add(new Product {
                    ProductTitle = "Chihuahua Dude",
                    ProductDescription = "Only *real* men own *really* tiny dogs.",
                    ImageUrl = "https://i.redd.it/ivd8p1o9xo021.jpg",
                    Cost = 15.00F,
                    Quantity = 612
                });

                _ctx.Products.Add(new Product {
                    ProductTitle = "BOOM TETRIS FOR JEFF",
                    ProductDescription = "Is your name Jeff? Do you play Tetris, occasionally? Boy, do we have the shirt for you!",
                    ImageUrl = "https://i.redd.it/ljmtxqvz3vz11.jpg",
                    Cost = 26.95F,
                    Quantity = 413
                });

                _ctx.Products.Add(new Product {
                    ProductTitle = "Everybody Makes Mistakes",
                    ProductDescription = "Can I borrow $20?",
                    ImageUrl = "https://i.redd.it/c7wwhkh60gw11.jpg",
                    Cost = 20.00F,
                    Quantity = 20
                });

                _ctx.SaveChanges();
            }
        }
    }
}
