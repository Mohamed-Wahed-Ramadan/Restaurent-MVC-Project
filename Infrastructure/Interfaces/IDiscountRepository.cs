using Models;

namespace Infrastructure.Interfaces
{
    public interface IDiscountRepository
    {
        Task<Discount> GetByIdAsync(int id);
        Task<List<Discount>> GetAllAsync();
        Task<List<Discount>> GetActiveDiscountsAsync();
        Task<Discount> CreateAsync(Discount discount);
        Task<Discount> UpdateAsync(Discount discount);
        Task DeleteAsync(int id);
        Task ToggleStatusAsync(int id, bool isActive);
    }
}
