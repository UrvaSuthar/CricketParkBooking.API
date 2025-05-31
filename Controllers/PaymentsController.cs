using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CricketParkBooking.API.Data;
using CricketParkBooking.API.Models;
using CricketParkBooking.API.Constants;
using CricketParkBooking.API.DTOs;
using CricketParkBooking.API.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CricketParkBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(ApplicationDbContext context, IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _context = context;
            _paymentService = paymentService;
            _logger = logger;
        }

        // GET: api/Payments
        [HttpGet]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAll()
        {
            var payments = await _paymentService.GetAllAsync();
            return Ok(payments);
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetById(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            // Check if the user is authorized to view this payment
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (!User.IsInRole(AppConstants.Roles.Admin))
            {
                return Forbid();
            }

            return Ok(payment);
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<ActionResult<PaymentDto>> Create(CreatePaymentDto createPaymentDto)
        {
            var payment = await _paymentService.CreateAsync(createPaymentDto);
            return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
        }

        // PUT: api/Payments/5
        [HttpPut("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<ActionResult<PaymentDto>> Update(int id, UpdatePaymentDto updatePaymentDto)
        {
            var payment = await _paymentService.UpdateAsync(id, updatePaymentDto);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _paymentService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetBookingPayments(int bookingId)
        {
            var payments = await _paymentService.GetBookingPaymentsAsync(bookingId);
            return Ok(payments);
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
} 