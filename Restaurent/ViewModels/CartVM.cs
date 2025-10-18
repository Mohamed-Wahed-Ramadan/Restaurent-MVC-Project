using System.ComponentModel.DataAnnotations;

namespace Restaurent.ViewModels
{
    public class CartVM
    {
        public int Id { get; set; }
        public int MenuProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }

    public class CartSummaryVM
    {
        public List<CartVM> CartItems { get; set; } = new List<CartVM>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public int TotalItems { get; set; }
    }

    public class CheckoutVM
    {
        [Required(ErrorMessage = "Please select order type")]
        public string OrderType { get; set; } // "DineIn" or "Delivery"

        [RequiredIf(nameof(OrderType), "DineIn", ErrorMessage = "Table number is required for dine-in orders")]
        public string? TableNumber { get; set; }

        [RequiredIf(nameof(OrderType), "Delivery", ErrorMessage = "Delivery address is required for delivery orders")]
        public string? DeliveryAddress { get; set; }

        public List<CartVM> CartItems { get; set; } = new List<CartVM>();
        public decimal Total { get; set; }
    }

    public class RequiredIfAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private object DesiredValue { get; set; }

        public RequiredIfAttribute(string propertyName, object desiredValue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();
            var propertyValue = type.GetProperty(PropertyName)?.GetValue(instance, null);

            if (propertyValue?.ToString() == DesiredValue.ToString() && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}