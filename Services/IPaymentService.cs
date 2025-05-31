using CricketParkBooking.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CricketParkBooking.API.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task<PaymentDto> GetByIdAsync(int id);
        Task<PaymentDto> CreateAsync(CreatePaymentDto createPaymentDto);
        Task<PaymentDto> UpdateAsync(int id, UpdatePaymentDto updatePaymentDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<PaymentDto>> GetBookingPaymentsAsync(int bookingId);
    }
} 