using AutoMapper;
using DTOs;
using Models;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category mappings
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<CategoryUpdateDTO, Category>();

            // User mappings
            CreateMap<User, UserDTO>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>();

            // MenuProduct mappings
            CreateMap<MenuProduct, MenuProductDTO>();
            CreateMap<MenuProductCreateDTO, MenuProduct>();
            CreateMap<MenuProductUpdateDTO, MenuProduct>();

            // Order mappings
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderCreateDTO, Order>();
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<OrderItemCreateDTO, OrderItem>();

            // Cart mappings
            CreateMap<Cart, CartDTO>();
            CreateMap<CartCreateDTO, Cart>();
            CreateMap<CartUpdateDTO, Cart>();

            // Favorite mappings
            CreateMap<Favorite, FavoriteDTO>();
            CreateMap<FavoriteCreateDTO, Favorite>();

            // Discount mappings
            CreateMap<Discount, DiscountDTO>();
            CreateMap<DiscountCreateDTO, Discount>();
            CreateMap<DiscountUpdateDTO, Discount>();
        }
    }
}