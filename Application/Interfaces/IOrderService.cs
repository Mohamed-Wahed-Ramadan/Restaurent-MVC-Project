using DTOs;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int id);
        Task<OrderDTO> GetOrderByUniqueIdAsync(string uniqueOrderId);
        Task<List<OrderDTO>> GetUserOrdersAsync(string userId);
        Task<List<OrderDTO>> GetRecentOrdersAsync(int count = 10);
        Task<OrderDTO> CreateOrderAsync(OrderCreateDTO orderDto);
        Task<OrderDTO> UpdateOrderStatusAsync(int orderId, string status);
        Task CancelOrderAsync(int orderId);
        Task<int> GetPendingOrdersCountAsync();
        Task<decimal> GetTodayRevenueAsync();
        Task CleanupOldOrdersAsync(int daysOld = 7);
        Task<int> CalculateEstimatedTimeAsync(List<CartDTO> cartItems, string orderType);
        Task<decimal> CalculateDiscountAsync(string userId, List<CartDTO> cartItems);
    }
}