using CricketParkBooking.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CricketParkBooking.API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(string id);
        Task<UserDto> RegisterAsync(CreateUserDto createUserDto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);
        Task<UserDto> UpdateAsync(string id, UpdateUserDto updateUserDto);
        Task DeleteAsync(string id);
    }
} 