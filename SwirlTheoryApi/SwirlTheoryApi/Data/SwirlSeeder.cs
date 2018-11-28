using Microsoft.AspNetCore.Identity;
using SwirlTheoryApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwirlTheoryApi.Data
{
    public class SwirlSeeder {
        private readonly ShoppingContext _ctx;
        private readonly UserManager<User> _userManager;

        public SwirlSeeder(ShoppingContext ctx, UserManager<User> userManager) {
            _ctx = ctx;
        }

        public async Task SeedAsync() {
            // NOTE: Should this be _ctx.Migrate() ?????
            _ctx.Database.EnsureCreated();
            User user = await _userManager.FindByEmailAsync("runtime@memeware.net");

            // Add a dummy user
            if (user == null) {
                user = new User()
                {
                    ProfileImageUrl = "https://ibb.co/XsW01rm",
                    Email = "runtime@memeware.net",
                    UserName = "runtime"
                };
            }

            var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
            if (result != IdentityResult.Success) {
                throw new InvalidOperationException("Could not create new user in seeder");
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

            // Add static lookup data to the OrderStatuses table
            /*
            // For when an order has been placed and not yet shipped
            _ctx.OrderStatuses.Add(new OrderStatus
            {
                Status = "Ordered"
            });

            // For once an order has been shipped from our warehouses
            _ctx.OrderStatuses.Add(new OrderStatus
            {
                Status = "Shipped"
            });

            // For after we think an order should have arrived to the customer
            _ctx.OrderStatuses.Add(new OrderStatus
            {
                Status = "Delivered"
            });
            */
        }
    }
}
