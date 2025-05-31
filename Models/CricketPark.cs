using System;
using System.Collections.Generic;

namespace CricketParkBooking.API.Models
{
    public class CricketPark
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public decimal PricePerHour { get; set; }
        public int NumberOfPitches { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<ParkManager> ParkManagers { get; set; }
    }
} 