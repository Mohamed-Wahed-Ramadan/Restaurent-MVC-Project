namespace DTOs
{
    public class AdminDashboardDTO
    {
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int TotalUsers { get; set; }
        public decimal TodayRevenue { get; set; }
        public List<OrderDTO> RecentOrders { get; set; } = new List<OrderDTO>();
    }
}