//using Application.Interfaces;
//using AutoMapper;
//using DTOs;
//using Infrastructure.Interfaces;

//namespace Application.Services
//{
//    public class FavoriteService : IFavoriteService
//    {
//        private readonly IFavoriteRepository _favoriteRepository;
//        private readonly IMapper _mapper;

//        public FavoriteService(IFavoriteRepository favoriteRepository, IMapper mapper)
//        {
//            _favoriteRepository = favoriteRepository;
//            _mapper = mapper;
//        }

//        public async Task<List<MenuProductDTO>> GetUserFavoritesAsync(string userId)
//        {
//            var favorites = await _favoriteRepository.GetByUserIdAsync(userId);
//            return favorites.Select(f => _mapper.Map<MenuProductDTO>(f.MenuProduct)).ToList();
//        }

//        public async Task<bool> ToggleFavoriteAsync(string userId, int productId)
//        {
//            var existingFavorite = await _favoriteRepository.GetByUserAndProductAsync(userId, productId);

//            if (existingFavorite != null)
//            {
//                await _favoriteRepository.DeleteAsync(existingFavorite.Id);
//                return false;
//            }
//            else
//            {
//                var favorite = new Models.Favorite
//                {
//                    UserId = userId,
//                    MenuProductId = productId,
//                    CreatedAt = DateTime.UtcNow
//                };
//                await _favoriteRepository.CreateAsync(favorite);
//                return true;
//            }
//        }

//        public async Task<bool> IsFavoriteAsync(string userId, int productId)
//        {
//            return await _favoriteRepository.IsFavoriteAsync(userId, productId);
//        }
//    }
//}