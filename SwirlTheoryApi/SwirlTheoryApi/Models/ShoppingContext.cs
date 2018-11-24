using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// An object which represents the database.
    /// </summary>
    public class ShoppingContext : DbContext {
        public ShoppingContext() { }
        public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options) {}

        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<BasketRow> BasketRows { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<DisplayLanguage> DisplayLanguages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}