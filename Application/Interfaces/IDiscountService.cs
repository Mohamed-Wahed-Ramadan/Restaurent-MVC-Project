using DTOs;

namespace Application.Interfaces
{
    public interface IDiscountService
    {
        Task<List<DiscountDTO>> GetAllDiscountsAsync();
        Task<DiscountDTO> GetDiscountByIdAsync(int id);
        Task<List<DiscountDTO>> GetActiveDiscountsAsync();
        Task<DiscountDTO> CreateDiscountAsync(DiscountCreateDTO discountDto);
        Task<DiscountDTO> UpdateDiscountAsync(DiscountUpdateDTO discountDto);
        Task DeleteDiscountAsync(int id);
        Task ToggleDiscountStatusAsync(int id);
        Task<decimal> CalculateDiscountForUserAsync(string userId, List<CartDTO> cartItems);
    }
}