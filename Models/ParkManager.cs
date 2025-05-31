using System;

namespace CricketParkBooking.API.Models
{
    public class ParkManager
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CricketParkId { get; set; }
        public DateTime AssignedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual CricketPark CricketPark { get; set; }
    }
} 