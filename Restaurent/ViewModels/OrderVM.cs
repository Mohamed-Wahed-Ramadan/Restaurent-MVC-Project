namespace Restaurent.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public string UniqueOrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public string OrderType { get; set; }
        public string Location { get; set; }
        public int EstimatedTime { get; set; }
        public List<OrderItemVM> OrderItems { get; set; } = new List<OrderItemVM>();
    }

    public class OrderItemVM
    {
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}