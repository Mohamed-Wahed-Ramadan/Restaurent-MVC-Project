using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//namespace Restaurent.Models
namespace Models
{
    public class Discount
    {
        [Key]
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

        public bool IsAgeBased { get; set; } = false;
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        // العلاقة مع الفئات
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        // إذا كان null يعني الخصم على كل المنتجات
        public bool ApplyToAllCategories => CategoryId == null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public bool IsValid => IsActive && DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    }
}