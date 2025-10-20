using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//namespace Restaurent.Models
namespace Models
{
    public class MenuProduct :BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        //public int ProductId { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int MinTime { get; set; }
        [Required]
        public int MaxTime { get; set; }
        public int? DayStock { get; set; } = 0;
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        //public Cart? Cart { get; set; }
        //public OrderCart? OrderCart { get; set; }
    }
}
