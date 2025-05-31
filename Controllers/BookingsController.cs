using CricketParkBooking.API.Constants;
using CricketParkBooking.API.DTOs;
using CricketParkBooking.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CricketParkBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Check if the user is authorized to view this booking
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (booking.UserId != userId && !User.IsInRole(AppConstants.Roles.Admin))
            {
                return Forbid();
            }

            return Ok(booking);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Create(CreateBookingDto createBookingDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var booking = await _bookingService.CreateAsync(createBookingDto, userId);
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<ActionResult<BookingDto>> Update(int id, UpdateBookingDto updateBookingDto)
        {
            var booking = await _bookingService.UpdateAsync(id, updateBookingDto);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookingService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetUserBookings()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(bookings);
        }
    }
} 