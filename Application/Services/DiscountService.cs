//using Application.Interfaces;
//using AutoMapper;
//using DTOs;
//using Infrastructure.Interfaces;
//using Models;

//namespace Application.Services
//{
//    public class DiscountService : IDiscountService
//    {
//        private readonly IDiscountRepository _discountRepository;
//        private readonly IMapper _mapper;

//        public DiscountService(IDiscountRepository discountRepository, IMapper mapper)
//        {
//            _discountRepository = discountRepository;
//            _mapper = mapper;
//        }

//        public async Task<List<DiscountDTO>> GetAllDiscountsAsync()
//        {
//            var discounts = await _discountRepository.GetAllAsync();
//            return _mapper.Map<List<DiscountDTO>>(discounts);
//        }

//        public async Task<DiscountDTO> GetDiscountByIdAsync(int id)
//        {
//            var discount = await _discountRepository.GetByIdAsync(id);
//            return _mapper.Map<DiscountDTO>(discount);
//        }

//        public async Task<List<DiscountDTO>> GetActiveDiscountsAsync()
//        {
//            var discounts = await _discountRepository.GetActiveDiscountsAsync();
//            return _mapper.Map<List<DiscountDTO>>(discounts);
//        }

//        public async Task<DiscountDTO> CreateDiscountAsync(DiscountCreateDTO discountDto)
//        {
//            var discount = _mapper.Map<Discount>(discountDto);
//            discount.CreatedAt = DateTime.UtcNow;
//            discount.IsActive = true;

//            var createdDiscount = await _discountRepository.CreateAsync(discount);
//            return _mapper.Map<DiscountDTO>(createdDiscount);
//        }

//        public async Task<DiscountDTO> UpdateDiscountAsync(DiscountUpdateDTO discountDto)
//        {
//            var existingDiscount = await _discountRepository.GetByIdAsync(discountDto.Id);
//            if (existingDiscount == null)
//                throw new ArgumentException("Discount not found");

//            existingDiscount.Name = discountDto.Name;
//            existingDiscount.Description = discountDto.Description;
//            existingDiscount.DiscountPercentage = discountDto.DiscountPercentage;
//            existingDiscount.IsAgeBased = discountDto.IsAgeBased;
//            existingDiscount.MinAge = discountDto.MinAge;
//            existingDiscount.MaxAge = discountDto.MaxAge;
//            existingDiscount.StartDate = discountDto.StartDate;
//            existingDiscount.EndDate = discountDto.EndDate;
//            existingDiscount.CategoryId = discountDto.CategoryId;
//            existingDiscount.UpdatedAt = DateTime.UtcNow;

//            var updatedDiscount = await _discountRepository.UpdateAsync(existingDiscount);
//            return _mapper.Map<DiscountDTO>(updatedDiscount);
//        }

//        public async Task DeleteDiscountAsync(int id)
//        {
//            await _discountRepository.DeleteAsync(id);
//        }

//        public async Task ToggleDiscountStatusAsync(int id)
//        {
//            var discount = await _discountRepository.GetByIdAsync(id);
//            if (discount == null)
//                throw new ArgumentException("Discount not found");

//            await _discountRepository.ToggleStatusAsync(id, !discount.IsActive);
//        }

//        public async Task<decimal> CalculateDiscountForUserAsync(string userId, List<CartDTO> cartItems)
//        {
//            // This would typically use user information and cart items to calculate applicable discounts
//            // For now, returning a simple calculation based on active discounts
//            var activeDiscounts = await GetActiveDiscountsAsync();
//            if (!activeDiscounts.Any()) return 0;

//            // Simple implementation - apply the highest discount percentage to the total
//            var highestDiscount = activeDiscounts.Max(d => d.DiscountPercentage);
//            var total = cartItems.Sum(c => c.Total);

//            return (total * highestDiscount) / 100;
//        }
//    }
//}