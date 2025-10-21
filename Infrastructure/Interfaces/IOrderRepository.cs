using Models;

namespace Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<Order> GetByUniqueIdAsync(string uniqueOrderId);
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetByUserIdAsync(string userId);
        Task<List<Order>> GetRecentAsync(int count = 10);
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<int> GetPendingCountAsync();
        Task<decimal> GetTodayRevenueAsync();
        Task CleanupOldOrdersAsync(int daysOld = 7);
    }
}