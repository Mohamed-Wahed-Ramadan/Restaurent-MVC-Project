//using Application.Interfaces;
//using AutoMapper;
//using DTOs;
//using Infrastructure.Interfaces;
//using Microsoft.AspNetCore.Identity;
//using Models;

//namespace Application.Services
//{
//    public class UserService : IUserService
//    {
//        private readonly IUserRepository _userRepository;
//        private readonly IMapper _mapper;

//        public UserService(IUserRepository userRepository, IMapper mapper)
//        {
//            _userRepository = userRepository;
//            _mapper = mapper;
//        }

//        public async Task<UserDTO> GetUserByIdAsync(string id)
//        {
//            var user = await _userRepository.GetByIdAsync(id);
//            return _mapper.Map<UserDTO>(user);
//        }

//        public async Task<UserDTO> GetUserByEmailAsync(string email)
//        {
//            var user = await _userRepository.GetByEmailAsync(email);
//            return _mapper.Map<UserDTO>(user);
//        }

//        public async Task<List<UserDTO>> GetAllUsersAsync()
//        {
//            var users = await _userRepository.GetAllAsync();
//            return _mapper.Map<List<UserDTO>>(users);
//        }

//        public async Task<List<UserDTO>> SearchUsersAsync(string search, string searchBy = "all")
//        {
//            var users = await _userRepository.SearchAsync(search, searchBy);
//            return _mapper.Map<List<UserDTO>>(users);
//        }

//        public async Task<UserDTO> CreateUserAsync(UserCreateDTO userDto, UserManager<User> userManager)
//        {
//            var user = new User
//            {
//                UserName = userDto.FullName,
//                Email = userDto.Email,
//                PhoneNumber = userDto.PhoneNumber,
//                Birthday = userDto.Birthday,
//                CreatedAt = DateTime.UtcNow
//            };

//            var result = await userManager.CreateAsync(user, userDto.Password);
//            if (!result.Succeeded)
//                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

//            return _mapper.Map<UserDTO>(user);
//        }

//        public async Task<UserDTO> UpdateUserAsync(UserUpdateDTO userDto, UserManager<User> userManager)
//        {
//            var user = await _userRepository.GetByIdAsync(userDto.Id);
//            if (user == null)
//                throw new ArgumentException("User not found");

//            user.UserName = userDto.UserName;
//            user.Email = userDto.Email;
//            user.PhoneNumber = userDto.PhoneNumber;
//            user.Birthday = userDto.Birthday;
//            user.UpdatedAt = DateTime.UtcNow;

//            if (!string.IsNullOrEmpty(userDto.NewPassword))
//            {
//                var token = await userManager.GeneratePasswordResetTokenAsync(user);
//                var result = await userManager.ResetPasswordAsync(user, token, userDto.NewPassword);
//                if (!result.Succeeded)
//                    throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
//            }

//            var updatedUser = await _userRepository.UpdateAsync(user);
//            return _mapper.Map<UserDTO>(updatedUser);
//        }

//        public async Task DeleteUserAsync(string id)
//        {
//            await _userRepository.DeleteAsync(id);
//        }

//        public async Task ToggleAdminStatusAsync(string id)
//        {
//            var user = await _userRepository.GetByIdAsync(id);
//            if (user == null)
//                throw new ArgumentException("User not found");

//            user.IsAdmin = !user.IsAdmin;
//            user.UpdatedAt = DateTime.UtcNow;
//            await _userRepository.UpdateAsync(user);
//        }

//        public async Task<bool> UserExistsAsync(string email)
//        {
//            return await _userRepository.ExistsAsync(email);
//        }
//    }
//}