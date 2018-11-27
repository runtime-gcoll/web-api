using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // See SwirlSeeder.cs for seed data
            /*
            builder.Entity<Product>()
                .HasData();
                */
        }
    }
}
