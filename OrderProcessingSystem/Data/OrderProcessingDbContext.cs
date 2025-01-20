using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OrderProcessingSystem.Data
{
    public class OrderProcessingDbContext : DbContext
    {
        public OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customer has many orders
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId);

            // Configure many-to-many relationship between Orders and Products
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany();
        }

        
    }
}
