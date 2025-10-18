using System.ComponentModel.DataAnnotations;

//namespace Restaurent.Models
namespace Models
{
    public abstract class BaseModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public int? IsUpdateBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int? IsDeletedBy { get; set; }
    }
}