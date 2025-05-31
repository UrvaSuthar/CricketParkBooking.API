using CricketParkBooking.API.Constants;
using CricketParkBooking.API.DTOs;
using CricketParkBooking.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CricketParkBooking.API.Controllers
{
    /// <summary>
    /// API controller for managing cricket park bookings
    /// </summary>
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

        /// <summary>
        /// Get all bookings
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves all bookings and is only accessible to administrators.
        /// </remarks>
        /// <returns>A list of all bookings</returns>
        /// <response code="200">Returns the list of bookings</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not an administrator</response>
        [HttpGet]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();
            return Ok(bookings);
        }

        /// <summary>
        /// Get a booking by ID
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves a specific booking by its ID.
        /// Users can only access their own bookings, while administrators can access any booking.
        /// </remarks>
        /// <param name="id">The ID of the booking to retrieve</param>
        /// <returns>The booking with the specified ID</returns>
        /// <response code="200">Returns the booking</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized to access this booking</response>
        /// <response code="404">If the booking is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Create a new booking
        /// </summary>
        /// <remarks>
        /// This endpoint creates a new booking for the authenticated user.
        /// </remarks>
        /// <param name="createBookingDto">The booking information</param>
        /// <returns>The newly created booking</returns>
        /// <response code="201">Returns the newly created booking</response>
        /// <response code="400">If the booking data is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BookingDto>> Create(CreateBookingDto createBookingDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var booking = await _bookingService.CreateAsync(createBookingDto, userId);
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }

        /// <summary>
        /// Update an existing booking
        /// </summary>
        /// <remarks>
        /// This endpoint updates an existing booking and is only accessible to administrators.
        /// </remarks>
        /// <param name="id">The ID of the booking to update</param>
        /// <param name="updateBookingDto">The updated booking information</param>
        /// <returns>The updated booking</returns>
        /// <response code="200">Returns the updated booking</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not an administrator</response>
        /// <response code="404">If the booking is not found</response>
        [HttpPut("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDto>> Update(int id, UpdateBookingDto updateBookingDto)
        {
            var booking = await _bookingService.UpdateAsync(id, updateBookingDto);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        /// <summary>
        /// Delete a booking
        /// </summary>
        /// <remarks>
        /// This endpoint deletes a booking and is only accessible to administrators.
        /// </remarks>
        /// <param name="id">The ID of the booking to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">If the booking is successfully deleted</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not an administrator</response>
        /// <response code="404">If the booking is not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookingService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Get bookings for the current user
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves all bookings for the authenticated user.
        /// </remarks>
        /// <returns>A list of the user's bookings</returns>
        /// <response code="200">Returns the list of the user's bookings</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetUserBookings()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(bookings);
        }
    }
} 