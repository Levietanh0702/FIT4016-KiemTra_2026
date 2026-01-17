using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    [Required, MaxLength(30)]
    public string OrderNumber { get; set; }

    [Required, MinLength(2), MaxLength(100)]
    public string CustomerName { get; set; }

    [Required, EmailAddress]
    public string CustomerEmail { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
