using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//namespace Restaurent.Models
namespace Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        public bool IsAdmin { get; set; } = false;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - Birthday.Year;
                if (Birthday.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public string? ImageURL { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Cart> CartItems { get; set; } = new List<Cart>();
        public List<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}