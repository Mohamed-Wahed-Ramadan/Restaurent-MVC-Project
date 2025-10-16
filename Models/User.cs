using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurent.Models
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

        // Computed Property - لا يُخزن في قاعدة البيانات
        [NotMapped]
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - Birthday.Year;

                // تحقق إذا لم يصل عيد ميلاده هذا العام
                if (Birthday.Date > today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
        }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
