using CricketParkBooking.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CricketParkBooking.API.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllAsync();
        Task<BookingDto> GetByIdAsync(int id);
        Task<BookingDto> CreateAsync(CreateBookingDto createBookingDto, string userId);
        Task<BookingDto> UpdateAsync(int id, UpdateBookingDto updateBookingDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<BookingDto>> GetUserBookingsAsync(string userId);
        Task<bool> IsTimeSlotAvailableAsync(int cricketParkId, DateTime date, TimeSpan startTime, TimeSpan endTime);
    }
} 