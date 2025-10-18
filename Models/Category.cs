//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

//namespace Restaurent.Models
namespace Models
{
    public class Category 
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; }
        public int? IsUpdateBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? IsDeletedBy { get; set; }
        public List<MenuProduct> MenuProduct { get; set; }
    }
}
