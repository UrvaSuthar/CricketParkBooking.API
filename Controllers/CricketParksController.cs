using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CricketParkBooking.API.Services;
using CricketParkBooking.API.DTOs;

namespace CricketParkBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CricketParksController : ControllerBase
    {
        private readonly ICricketParkService _cricketParkService;
        private readonly ILogger<CricketParksController> _logger;

        public CricketParksController(
            ICricketParkService cricketParkService,
            ILogger<CricketParksController> logger)
        {
            _cricketParkService = cricketParkService;
            _logger = logger;
        }

        // GET: api/CricketParks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CricketParkDto>>> GetCricketParks()
        {
            var cricketParks = await _cricketParkService.GetAllAsync();
            return Ok(cricketParks);
        }

        // GET: api/CricketParks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CricketParkDto>> GetCricketPark(int id)
        {
            var cricketPark = await _cricketParkService.GetByIdAsync(id);
            if (cricketPark == null)
            {
                return NotFound();
            }
            return Ok(cricketPark);
        }

        // GET: api/CricketParks/search?term=searchTerm
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CricketParkDto>>> SearchCricketParks([FromQuery] string term)
        {
            var cricketParks = await _cricketParkService.SearchAsync(term);
            return Ok(cricketParks);
        }

        // POST: api/CricketParks
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CricketParkDto>> CreateCricketPark(CreateCricketParkDto createDto)
        {
            var cricketPark = await _cricketParkService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetCricketPark), new { id = cricketPark.Id }, cricketPark);
        }

        // PUT: api/CricketParks/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCricketPark(int id, UpdateCricketParkDto updateDto)
        {
            try
            {
                await _cricketParkService.UpdateAsync(id, updateDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/CricketParks/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCricketPark(int id)
        {
            try
            {
                await _cricketParkService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
} 