using CricketParkBooking.API.DTOs;

namespace CricketParkBooking.API.Services
{
    public interface ICricketParkService
    {
        Task<IEnumerable<CricketParkDto>> GetAllAsync();
        Task<CricketParkDto> GetByIdAsync(int id);
        Task<CricketParkDto> CreateAsync(CreateCricketParkDto createDto);
        Task UpdateAsync(int id, UpdateCricketParkDto updateDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<CricketParkDto>> SearchAsync(string searchTerm);
    }
} 