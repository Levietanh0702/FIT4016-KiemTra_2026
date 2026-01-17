using Microsoft.EntityFrameworkCore;

public class OrderDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(
            "Server=.;Database=OrderManagement;Trusted_Connection=True;TrustServerCertificate=True"
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name).IsUnique();

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Sku).IsUnique();

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.OrderNumber).IsUnique();

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.CustomerEmail).IsUnique();

        // ===== SEED PRODUCTS =====
        var products = new List<Product>();
        for (int i = 1; i <= 15; i++)
        {
            products.Add(new Product
            {
                Id = i,
                Name = $"Product {i}",
                Sku = $"SKU{i:000}",
                Price = 100 + i,
                StockQuantity = 100,
                Category = "General"
            });
        }
        modelBuilder.Entity<Product>().HasData(products);

        // ===== SEED ORDERS =====
        var orders = new List<Order>();
        for (int i = 1; i <= 30; i++)
        {
            orders.Add(new Order
            {
                Id = i,
                ProductId = (i % 15) + 1,
                OrderNumber = $"ORD-20260117-{i:0000}",
                CustomerName = $"Customer {i}",
                CustomerEmail = $"customer{i}@mail.com",
                Quantity = 1 + (i % 5),
                OrderDate = DateTime.Today.AddDays(-i)
            });
        }
        modelBuilder.Entity<Order>().HasData(orders);
    }
}
