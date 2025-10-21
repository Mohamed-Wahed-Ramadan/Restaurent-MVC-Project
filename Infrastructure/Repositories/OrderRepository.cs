using Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDpContext _context;

        public OrderRepository(AppDpContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuProduct)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> GetByUniqueIdAsync(string uniqueOrderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuProduct)
                .FirstOrDefaultAsync(o => o.UniqueOrderId == uniqueOrderId);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuProduct)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Order>> GetByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuProduct)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Order>> GetRecentAsync(int count = 10)
        {
            return await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<int> GetPendingCountAsync()
        {
            return await _context.Orders
                .CountAsync(o => o.Status == "Pending");
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.Orders
                .Where(o => o.CreatedAt >= today && o.CreatedAt < tomorrow)
                .SumAsync(o => (decimal?)o.Total) ?? 0;
        }

        public async Task CleanupOldOrdersAsync(int daysOld = 7)
        {
            var cutoffDate = DateTime.Now.AddDays(-daysOld);
            var oldCompletedOrders = await _context.Orders
                .Where(o => o.Status == "Completed" && o.CreatedAt < cutoffDate)
                .ToListAsync();

            if (oldCompletedOrders.Any())
            {
                _context.Orders.RemoveRange(oldCompletedOrders);
                await _context.SaveChangesAsync();
            }
        }
    }
}