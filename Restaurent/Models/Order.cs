using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurent.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Preparing, Ready, Completed, Cancelled

        public int TimePreparing { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string OrderType { get; set; } // "DineIn" or "Delivery"

        public string? TableNumber { get; set; } // For dine-in orders
        public string? DeliveryAddress { get; set; } // For delivery orders

        [Required]
        public string UniqueOrderId { get; set; } = GenerateUniqueOrderId();

        public List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();

        // تأكد أن الدالة static
        public static string GenerateUniqueOrderId()
        {
            return $"ORD-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}