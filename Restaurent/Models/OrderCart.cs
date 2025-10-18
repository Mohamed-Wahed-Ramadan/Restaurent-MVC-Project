using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurent.Models
{
    public class OrderCart
    {
        [Key]
        public int Id { get; set; }
        //public int UserId { get; set; }
        [ForeignKey("MenuProduct")]
        public int MenuProductId { get; set; }
        public int Mount { get; set; }
        public decimal Total { get; set; }
        [ForeignKey("Cart")]
        public int  CartId { get; set; }
        public string UniqueOrderId { get; set; }
        public Order Order { get; set; }
        public List<Cart> Carts { get; set; }
        public List<MenuProduct> MenuProducts { get; set; }

    }
}
