using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } // تغيير إلى string

        [Required]
        public decimal Total { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        public int TimePreparing { get; set; }

        [Required]
        public string OrderType { get; set; } // Delivery, DineIn, Takeaway

        public string? Location { get; set; }
        public int? TableNumber { get; set; }
        public string? DeliveryAddress { get; set; }

        [Required]
        public string UniqueOrderId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}