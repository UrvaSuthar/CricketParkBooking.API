using System;
using CricketParkBooking.API.Enums;

namespace CricketParkBooking.API.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CricketParkId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public User User { get; set; }
        public CricketPark CricketPark { get; set; }
        public Payment Payment { get; set; }
    }
} 