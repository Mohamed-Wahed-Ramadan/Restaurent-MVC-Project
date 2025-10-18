using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Restaurent.ViewModels
{
    public class MenuCreVw
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000, ErrorMessage = "Price must be between 0.01 and 1000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, 1000, ErrorMessage = "Quantity must be between 0 and 1000")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Minimum time is required")]
        [Range(1, 120, ErrorMessage = "Minimum time must be between 1 and 120 minutes")]
        public int MinTime { get; set; }

        [Required(ErrorMessage = "Maximum time is required")]
        [Range(1, 120, ErrorMessage = "Maximum time must be between 1 and 120 minutes")]
        public int MaxTime { get; set; }

        [Range(0, 1000, ErrorMessage = "Day stock must be between 0 and 1000")]
        public int? DayStock { get; set; } = 0;

        public IEnumerable<SelectListItem>? Categories { get; set; }

        // للحقل الجديد لرفع الصور
        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }
    }
}