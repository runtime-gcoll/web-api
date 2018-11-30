using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SwirlTheoryApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwirlTheoryApi.Data
{
    /// <summary>
    /// An object which represents the database.
    /// </summary>
    public class ShoppingContext : IdentityDbContext<User> {
        public ShoppingContext() { }
        public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options) {}

        public DbSet<Address> Addresses { get; set; }
        public DbSet<BasketRow> BasketRows { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public new DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // See SwirlSeeder.cs for seed data
            /*
            builder.Entity<Product>()
                .HasData();
                */

            // Set up value conversion for the OrderStatus type
            // This will allow EF Core to map DB values to the enum in our Entities namespace (OrderStatus.cs)
            builder.Entity<Order>()
                .Property(e => e.OrderStatus)
                .HasConversion<string>();
        }
    }
}
