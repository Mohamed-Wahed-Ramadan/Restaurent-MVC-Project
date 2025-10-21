using Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDpContext _context;

        public UserRepository(AppDpContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>> SearchAsync(string search, string searchBy = "all")
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();

                switch (searchBy.ToLower())
                {
                    case "name":
                        query = query.Where(u => u.UserName.ToLower().Contains(search));
                        break;
                    case "email":
                        query = query.Where(u => u.Email.ToLower().Contains(search));
                        break;
                    case "phone":
                        query = query.Where(u => u.PhoneNumber.Contains(search));
                        break;
                    case "all":
                    default:
                        query = query.Where(u =>
                            u.UserName.ToLower().Contains(search) ||
                            u.Email.ToLower().Contains(search) ||
                            u.PhoneNumber.Contains(search));
                        break;
                }
            }

            return await query.ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}