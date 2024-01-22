using Microsoft.EntityFrameworkCore;
using ShopRunner.Entities;

namespace ShopRunner.DatabaseContexts
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public required DbSet<User> Users { get; set; }
        public required DbSet<Category> Categories { get; set; }
        public required DbSet<Product> Products { get; set; }
        public required DbSet<Color>   Colors { get; set; }
        public required DbSet<Size> Sizes { get; set; } 

        public required DbSet<Order> Orders { get; set; }
        public required DbSet<OrderItems> Items { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
