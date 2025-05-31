using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CricketParkBooking.API.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<ParkManager> ManagedParks { get; set; }
    }
} 