using Models;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
        Task<List<User>> SearchAsync(string search, string searchBy = "all");
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string email);
    }
}