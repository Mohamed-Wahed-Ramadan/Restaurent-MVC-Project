using System.ComponentModel.DataAnnotations;

namespace Restaurent.ViewModels
{
    public class CheckoutVm
    {
        // الخصائص الحالية
        public List<CartVM> CartItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        // إضافة خصائص التخفيض
        public decimal DiscountAmount { get; set; }
        public string AppliedDiscountName { get; set; }

        // باقي الخصائص مع Data Annotations
        [Required(ErrorMessage = "Please select order type")]
        public string OrderType { get; set; }

        public int? TableNumber { get; set; }

        public string DeliveryAddress { get; set; }
    }
}