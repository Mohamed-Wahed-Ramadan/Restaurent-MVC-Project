using DTOs;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(string id);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<List<UserDTO>> SearchUsersAsync(string search, string searchBy = "all");
        Task<UserDTO> CreateUserAsync(UserCreateDTO userDto, UserManager<Models.User> userManager);
        Task<UserDTO> UpdateUserAsync(UserUpdateDTO userDto, UserManager<Models.User> userManager);
        Task DeleteUserAsync(string id);
        Task ToggleAdminStatusAsync(string id);
        Task<bool> UserExistsAsync(string email);
    }
}