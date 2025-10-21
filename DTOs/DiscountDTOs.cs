using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DiscountDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool IsAgeBased { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int? CategoryId { get; set; }
        public CategoryDTO Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsValid { get; set; }
    }

    public class DiscountCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(1, 100)]
        public decimal DiscountPercentage { get; set; }

        public bool IsAgeBased { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int? CategoryId { get; set; }
    }

    public class DiscountUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(1, 100)]
        public decimal DiscountPercentage { get; set; }

        public bool IsAgeBased { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int? CategoryId { get; set; }
    }
}