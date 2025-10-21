using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MenuProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public MenuProductDTO MenuProduct { get; set; }
    }

    public class CartCreateDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int MenuProductId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }

    public class CartUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}