using System.ComponentModel.DataAnnotations;

namespace Restaurent.Models
{
    public abstract class BaseModel
    {
        [Key]
        [Required]
        public int Id { get; set; }  // Changed to public

        //[Required]
        //[StringLength(50)]
        //public string Name { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Changed name from CreateAt

        public DateTime? UpdatedAt { get; set; }  // Changed name from UpdateAt

        public int? IsUpdateBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int? IsDeletedBy { get; set; }
    }
}