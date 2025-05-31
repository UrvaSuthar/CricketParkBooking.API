using AutoMapper;
using CricketParkBooking.API.Constants;
using CricketParkBooking.API.DTOs;
using CricketParkBooking.API.Enums;
using CricketParkBooking.API.Models;
using CricketParkBooking.API.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CricketParkBooking.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IGenericRepository<Payment> paymentRepository,
            IGenericRepository<Booking> bookingRepository,
            IMapper mapper,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            try
            {
                var payments = await _paymentRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<PaymentDto>>(payments.Where(p => p.IsActive));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all payments");
                throw;
            }
        }

        public async Task<PaymentDto> GetByIdAsync(int id)
        {
            try
            {
                var payment = await _paymentRepository.GetByIdAsync(id);
                if (payment == null || !payment.IsActive)
                {
                    return null;
                }
                return _mapper.Map<PaymentDto>(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting payment with ID {PaymentId}", id);
                throw;
            }
        }

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto createPaymentDto)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(createPaymentDto.BookingId);
                if (booking == null || !booking.IsActive)
                {
                    throw new KeyNotFoundException(AppConstants.ErrorMessages.NotFound);
                }

                var payment = _mapper.Map<Payment>(createPaymentDto);
                payment.Status = PaymentStatus.Pending;
                payment.CreatedAt = DateTime.UtcNow;
                payment.IsActive = true;

                var createdPayment = await _paymentRepository.AddAsync(payment);
                return _mapper.Map<PaymentDto>(createdPayment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating payment for booking {BookingId}", createPaymentDto.BookingId);
                throw;
            }
        }

        public async Task<PaymentDto> UpdateAsync(int id, UpdatePaymentDto updatePaymentDto)
        {
            try
            {
                var payment = await _paymentRepository.GetByIdAsync(id);
                if (payment == null || !payment.IsActive)
                {
                    throw new KeyNotFoundException(AppConstants.ErrorMessages.NotFound);
                }

                _mapper.Map(updatePaymentDto, payment);
                payment.UpdatedAt = DateTime.UtcNow;

                await _paymentRepository.UpdateAsync(payment);
                return _mapper.Map<PaymentDto>(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating payment {PaymentId}", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var payment = await _paymentRepository.GetByIdAsync(id);
                if (payment == null || !payment.IsActive)
                {
                    throw new KeyNotFoundException(AppConstants.ErrorMessages.NotFound);
                }

                payment.IsActive = false;
                payment.UpdatedAt = DateTime.UtcNow;
                await _paymentRepository.UpdateAsync(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting payment {PaymentId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<PaymentDto>> GetBookingPaymentsAsync(int bookingId)
        {
            try
            {
                var payments = await _paymentRepository.GetAllAsync();
                var bookingPayments = payments.Where(p => p.BookingId == bookingId && p.IsActive);
                return _mapper.Map<IEnumerable<PaymentDto>>(bookingPayments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting payments for booking {BookingId}", bookingId);
                throw;
            }
        }
    }
} 