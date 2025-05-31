using AutoMapper;
using CricketParkBooking.API.DTOs;
using CricketParkBooking.API.Models;

namespace CricketParkBooking.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // CricketPark mappings
            CreateMap<CricketPark, CricketParkDto>();
            CreateMap<CreateCricketParkDto, CricketPark>();
            CreateMap<UpdateCricketParkDto, CricketPark>();

            // Booking mappings
            CreateMap<Booking, BookingDto>();
            CreateMap<CreateBookingDto, Booking>();
            CreateMap<UpdateBookingDto, Booking>();

            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            // Payment mappings
            CreateMap<Payment, PaymentDto>();
            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<UpdatePaymentDto, Payment>();
        }
    }
} 