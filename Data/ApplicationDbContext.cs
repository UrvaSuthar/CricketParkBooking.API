using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CricketParkBooking.API.Models;

namespace CricketParkBooking.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CricketPark> CricketParks { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ParkManager> ParkManagers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for currency properties
            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CricketPark>()
                .Property(cp => cp.PricePerHour)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            // Configure relationships and constraints
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.CricketPark)
                .WithMany(cp => cp.Bookings)
                .HasForeignKey(b => b.CricketParkId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ParkManager>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.ManagedParks)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ParkManager>()
                .HasOne(pm => pm.CricketPark)
                .WithMany(cp => cp.ParkManagers)
                .HasForeignKey(pm => pm.CricketParkId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed default roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "ParkManager", NormalizedName = "PARKMANAGER" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            );
        }
    }
} 