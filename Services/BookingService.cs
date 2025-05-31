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
    public class BookingService : IBookingService
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<CricketPark> _cricketParkRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IGenericRepository<Booking> bookingRepository,
            IGenericRepository<CricketPark> cricketParkRepository,
            IMapper mapper,
            ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _cricketParkRepository = cricketParkRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BookingDto>> GetAllAsync()
        {
            try
            {
                var bookings = await _bookingRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<BookingDto>>(bookings.Where(b => b.IsActive));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all bookings");
                throw;
            }
        }

        public async Task<BookingDto> GetByIdAsync(int id)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(id);
                if (booking == null || !booking.IsActive)
                {
                    return null;
                }
                return _mapper.Map<BookingDto>(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting booking with ID {BookingId}", id);
                throw;
            }
        }

        public async Task<BookingDto> CreateAsync(CreateBookingDto createBookingDto, string userId)
        {
            try
            {
                var cricketPark = await _cricketParkRepository.GetByIdAsync(createBookingDto.CricketParkId);
                if (cricketPark == null || !cricketPark.IsActive)
                {
                    throw new KeyNotFoundException(AppConstants.ErrorMessages.NotFound);
                }

                if (!await IsTimeSlotAvailableAsync(createBookingDto.CricketParkId, createBookingDto.BookingDate, createBookingDto.StartTime, createBookingDto.EndTime))
                {
                    throw new InvalidOperationException(AppConstants.ErrorMessages.SlotNotAvailable);
                }

                var booking = _mapper.Map<Booking>(createBookingDto);
                booking.UserId = userId;
                booking.Status = BookingStatus.Pending;
                booking.CreatedAt = DateTime.UtcNow;
                booking.IsActive = true;

                var createdBooking = await _bookingRepository.AddAsync(booking);
                return _mapper.Map<BookingDto>(createdBooking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating booking for user {UserId}", userId);
                throw;
            }
        }

        public async Task<BookingDto> UpdateAsync(int id, UpdateBookingDto updateBookingDto)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(id);
                if (booking == null || !booking.IsActive)
                {
                    throw new KeyNotFoundException(AppConstants.ErrorMessages.NotFound);
                }

                _mapper.Map(updateBookingDto, booking);
                booking.UpdatedAt = DateTime.UtcNow;

                await _bookingRepository.UpdateAsync(booking);
                return _mapper.Map<BookingDto>(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating booking {BookingId}", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(id);
                if (booking == null || !booking.IsActive)
                {
                    throw new KeyNotFoundException(AppConstants.ErrorMessages.NotFound);
                }

                booking.IsActive = false;
                booking.UpdatedAt = DateTime.UtcNow;
                await _bookingRepository.UpdateAsync(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting booking {BookingId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(string userId)
        {
            try
            {
                var bookings = await _bookingRepository.GetAllAsync();
                var userBookings = bookings.Where(b => b.UserId == userId && b.IsActive);
                return _mapper.Map<IEnumerable<BookingDto>>(userBookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting bookings for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> IsTimeSlotAvailableAsync(int cricketParkId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            try
            {
                var bookings = await _bookingRepository.GetAllAsync();
                var conflictingBookings = bookings.Where(b =>
                    b.CricketParkId == cricketParkId &&
                    b.IsActive &&
                    b.BookingDate.Date == date.Date &&
                    b.Status != BookingStatus.Cancelled &&
                    ((startTime >= b.StartTime && startTime < b.EndTime) ||
                     (endTime > b.StartTime && endTime <= b.EndTime) ||
                     (startTime <= b.StartTime && endTime >= b.EndTime)));

                return !conflictingBookings.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking time slot availability for cricket park {CricketParkId}", cricketParkId);
                throw;
            }
        }
    }
} 