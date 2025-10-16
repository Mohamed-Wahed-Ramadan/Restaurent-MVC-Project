using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Restaurent.ViewModels
{
    public class MenuCreVw
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, 1000, ErrorMessage = "Quantity must be between 0 and 1000")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Minimum preparation time is required")]
        [Range(1, 300, ErrorMessage = "Min time must be between 1 and 300 minutes")]
        public int MinTime { get; set; }

        [Required(ErrorMessage = "Maximum preparation time is required")]
        [Range(1, 300, ErrorMessage = "Max time must be between 1 and 300 minutes")]
        public int MaxTime { get; set; }

        [Range(0, 1000, ErrorMessage = "Day stock must be between 0 and 1000")]
        public int? DayStock { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public SelectList? Categories { get; set; }
    }
}