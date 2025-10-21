using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class MenuProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int MinTime { get; set; }
        public int MaxTime { get; set; }
        public int? DayStock { get; set; }
        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class MenuProductCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 1000)]
        public int Quantity { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Range(1, 120)]
        public int MinTime { get; set; }

        [Required]
        [Range(1, 120)]
        public int MaxTime { get; set; }

        [Range(0, 1000)]
        public int? DayStock { get; set; }

        public IFormFile ImageFile { get; set; }
    }

    public class MenuProductUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 1000)]
        public int Quantity { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Range(1, 120)]
        public int MinTime { get; set; }

        [Required]
        [Range(1, 120)]
        public int MaxTime { get; set; }

        [Range(0, 1000)]
        public int? DayStock { get; set; }

        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
    }
}