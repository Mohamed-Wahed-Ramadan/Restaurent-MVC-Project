using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public int TimePreparing { get; set; }
        public string OrderType { get; set; }
        public string Location { get; set; }
        public int? TableNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public string UniqueOrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UserDTO User { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public class OrderCreateDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public string OrderType { get; set; }

        public string Location { get; set; }
        public int? TableNumber { get; set; }
        public string DeliveryAddress { get; set; }

        [Required]
        public List<OrderItemCreateDTO> OrderItems { get; set; }
    }

    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public MenuProductDTO MenuProduct { get; set; }
    }

    public class OrderItemCreateDTO
    {
        [Required]
        public int MenuProductId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
    }
}