namespace CricketParkBooking.API.DTOs
{
    public class CricketParkDto
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
    }

    public class CreateCricketParkDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public decimal PricePerHour { get; set; }
        public int NumberOfPitches { get; set; }
    }

    public class UpdateCricketParkDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public decimal PricePerHour { get; set; }
        public int NumberOfPitches { get; set; }
    }
} 