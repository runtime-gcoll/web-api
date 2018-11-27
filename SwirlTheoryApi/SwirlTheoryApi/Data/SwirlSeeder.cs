using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace SwirlTheoryApi.Data
{
    public class SwirlSeeder {
        private readonly ShoppingContext _ctx;

        public SwirlSeeder(ShoppingContext ctx) {
            _ctx = ctx;
        }

        public void Seed() {
            // NOTE: Should this be _ctx.Migrate() ?????
            _ctx.Database.EnsureCreated();

            // Add dummy product data for testing
            if (!_ctx.Products.Any()) {
                // Need to create sample data

                // Add three example products
                _ctx.Products.Add(new Product {
                    //ProductId = 1,
                    ProductTitle = "Carolina Avengers",
                    ProductDescription = "A totally WIZARD shirt for the sports nerd in your life!",
                    ImageUrl = "https://i.redd.it/ogmlb210je021.jpg",
                    Cost = 35.00F
                });

                _ctx.Products.Add(new Product {
                    //ProductId = 2,
                    ProductTitle = "Chihuahua Dude",
                    ProductDescription = "Only *real* men own *really* tiny dogs.",
                    ImageUrl = "https://i.redd.it/ivd8p1o9xo021.jpg",
                    Cost = 15.00F
                });

                _ctx.Products.Add(new Product {
                    //ProductId = 3,
                    ProductTitle = "BOOM TETRIS FOR JEFF",
                    ProductDescription = "Is your name Jeff? Do you play Tetris, occasionally? Boy, do we have the shirt for you!",
                    ImageUrl = "https://i.redd.it/ljmtxqvz3vz11.jpg",
                    Cost = 26.95F
                });

                _ctx.Products.Add(new Product {
                    //ProductId = 4,
                    ProductTitle = "Everybody Makes Mistakes",
                    ProductDescription = "Can I borrow $20?",
                    ImageUrl = "https://i.redd.it/ljmtxqvz3vz11.jpg",
                    Cost = 20.00F
                });

                _ctx.SaveChanges();
            }

            // TODO: Add lookup data to tables here (OrderStatuses)
        }
    }
}
