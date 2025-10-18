using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurent.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("MenuProduct")]
        public int MenuProductId { get; set; }
        public MenuProduct MenuProduct { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}