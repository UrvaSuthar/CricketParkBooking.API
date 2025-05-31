using CricketParkBooking.API.Enums;

namespace CricketParkBooking.API.DTOs
{
    public class BookingDto
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
        public CricketParkDto CricketPark { get; set; }
        public PaymentDto Payment { get; set; }
    }

    public class CreateBookingDto
    {
        public int CricketParkId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public class UpdateBookingDto
    {
        public BookingStatus Status { get; set; }
    }
} 