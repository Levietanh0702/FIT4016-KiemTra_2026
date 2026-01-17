using Microsoft.EntityFrameworkCore;

// ================== DATABASE ==================
using var db = new OrderDbContext();


// ================== MAIN MENU ==================
while (true)
{
    Console.WriteLine("===== ORDER MANAGEMENT =====");
    Console.WriteLine("1. Create Order");
    Console.WriteLine("2. List Orders");
    Console.WriteLine("3. Update Order");
    Console.WriteLine("4. Delete Order");
    Console.WriteLine("0. Exit");
    Console.Write("Choose: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            CreateOrder(db);
            break;
        case "2":
            ListOrders(db);
            break;
        case "3":
            UpdateOrder(db);
            break;
        case "4":
            DeleteOrder(db);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}

// ================== FUNCTIONS ==================

void CreateOrder(OrderDbContext db)
{
    Console.Write("Order Number: ");
    string orderNumber = Console.ReadLine();

    if (!System.Text.RegularExpressions.Regex.IsMatch(orderNumber, @"^ORD-\d{8}-\d{4}$"))
    {
        Console.WriteLine("Invalid Order Number format.");
        return;
    }

    Console.Write("Customer Name: ");
    string name = Console.ReadLine();

    Console.Write("Customer Email: ");
    string email = Console.ReadLine();

    Console.Write("Product ID: ");
    if (!int.TryParse(Console.ReadLine(), out int productId))
    {
        Console.WriteLine("Invalid Product ID.");
        return;
    }

    var product = db.Products.Find(productId);
    if (product == null)
    {
        Console.WriteLine("Product does not exist.");
        return;
    }

    Console.Write("Quantity: ");
    if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0 || qty > product.StockQuantity)
    {
        Console.WriteLine("Invalid quantity.");
        return;
    }

    var order = new Order
    {
        OrderNumber = orderNumber,
        CustomerName = name,
        CustomerEmail = email,
        ProductId = productId,
        Quantity = qty,
        OrderDate = DateTime.Now
    };

    db.Orders.Add(order);
    db.SaveChanges();

    Console.WriteLine("Order created successfully.");
}

void ListOrders(OrderDbContext db)
{
    var orders = db.Orders
        .Include(o => o.Product)
        .OrderBy(o => o.Id)
        .ToList();

    Console.WriteLine("OrderNumber | Customer | Email | Product | Qty | Date | Status");

    foreach (var o in orders)
    {
        string status = o.DeliveryDate == null ? "Pending" : "Delivered";
        Console.WriteLine(
            $"{o.OrderNumber} | {o.CustomerName} | {o.CustomerEmail} | {o.Product.Name} | {o.Quantity} | {o.OrderDate:yyyy-MM-dd} | {status}"
        );
    }

    Console.WriteLine($"Total Orders: {orders.Count}");
}

void UpdateOrder(OrderDbContext db)
{
    Console.Write("Order ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var order = db.Orders.Find(id);
    if (order == null)
    {
        Console.WriteLine("Order not found.");
        return;
    }

    Console.Write("New Customer Name: ");
    order.CustomerName = Console.ReadLine();

    Console.Write("New Email: ");
    order.CustomerEmail = Console.ReadLine();

    Console.Write("New Quantity: ");
    if (!int.TryParse(Console.ReadLine(), out int qty))
    {
        Console.WriteLine("Invalid quantity.");
        return;
    }

    order.Quantity = qty;

    Console.Write("Delivery Date (yyyy-MM-dd or empty): ");
    var input = Console.ReadLine();
    if (!string.IsNullOrEmpty(input))
        order.DeliveryDate = DateTime.Parse(input);

    db.SaveChanges();
    Console.WriteLine("Order updated successfully.");
}

void DeleteOrder(OrderDbContext db)
{
    Console.Write("Order ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var order = db.Orders.Find(id);
    if (order == null)
    {
        Console.WriteLine("Order not found.");
        return;
    }

    Console.Write("Confirm delete (y/n): ");
    if (Console.ReadLine()?.ToLower() == "y")
    {
        db.Orders.Remove(order);
        db.SaveChanges();
        Console.WriteLine("Order deleted successfully.");
    }
}
