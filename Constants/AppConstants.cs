namespace CricketParkBooking.API.Constants
{
    public static class AppConstants
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string ParkManager = "ParkManager";
            public const string User = "User";
        }

        public static class BookingStatus
        {
            public const string Pending = "Pending";
            public const string Confirmed = "Confirmed";
            public const string Cancelled = "Cancelled";
            public const string Completed = "Completed";
        }

        public static class PaymentStatus
        {
            public const string Pending = "Pending";
            public const string Paid = "Paid";
            public const string Failed = "Failed";
            public const string Refunded = "Refunded";
        }

        public static class PaymentMethod
        {
            public const string CreditCard = "CreditCard";
            public const string DebitCard = "DebitCard";
            public const string UPI = "UPI";
            public const string NetBanking = "NetBanking";
        }

        public static class ErrorMessages
        {
            public const string NotFound = "Resource not found";
            public const string Unauthorized = "Unauthorized access";
            public const string InvalidInput = "Invalid input data";
            public const string DuplicateEmail = "Email already exists";
            public const string InvalidCredentials = "Invalid email or password";
            public const string SlotNotAvailable = "The selected time slot is not available";
        }
    }
} 