using AutoMapper;
using CricketParkBooking.API.DTOs;
using CricketParkBooking.API.Models;
using CricketParkBooking.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CricketParkBooking.API.Services
{
    public class CricketParkService : ICricketParkService
    {
        private readonly IGenericRepository<CricketPark> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CricketParkService> _logger;

        public CricketParkService(
            IGenericRepository<CricketPark> repository,
            IMapper mapper,
            ILogger<CricketParkService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CricketParkDto>> GetAllAsync()
        {
            try
            {
                var cricketParks = await _repository.FindAsync(cp => cp.IsActive);
                return _mapper.Map<IEnumerable<CricketParkDto>>(cricketParks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all cricket parks");
                throw;
            }
        }

        public async Task<CricketParkDto> GetByIdAsync(int id)
        {
            try
            {
                var cricketPark = await _repository.GetByIdAsync(id);
                if (cricketPark == null || !cricketPark.IsActive)
                {
                    return null;
                }
                return _mapper.Map<CricketParkDto>(cricketPark);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting cricket park with id {Id}", id);
                throw;
            }
        }

        public async Task<CricketParkDto> CreateAsync(CreateCricketParkDto createDto)
        {
            try
            {
                var cricketPark = _mapper.Map<CricketPark>(createDto);
                cricketPark.CreatedAt = DateTime.UtcNow;
                cricketPark.IsActive = true;

                var createdPark = await _repository.AddAsync(cricketPark);
                return _mapper.Map<CricketParkDto>(createdPark);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating cricket park");
                throw;
            }
        }

        public async Task UpdateAsync(int id, UpdateCricketParkDto updateDto)
        {
            try
            {
                var existingPark = await _repository.GetByIdAsync(id);
                if (existingPark == null || !existingPark.IsActive)
                {
                    throw new KeyNotFoundException($"Cricket park with id {id} not found");
                }

                _mapper.Map(updateDto, existingPark);
                existingPark.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(existingPark);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating cricket park with id {Id}", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var cricketPark = await _repository.GetByIdAsync(id);
                if (cricketPark == null)
                {
                    throw new KeyNotFoundException($"Cricket park with id {id} not found");
                }

                cricketPark.IsActive = false;
                cricketPark.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(cricketPark);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting cricket park with id {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<CricketParkDto>> SearchAsync(string searchTerm)
        {
            try
            {
                var cricketParks = await _repository.FindAsync(cp =>
                    cp.IsActive &&
                    (cp.Name.Contains(searchTerm) ||
                     cp.City.Contains(searchTerm) ||
                     cp.State.Contains(searchTerm)));

                return _mapper.Map<IEnumerable<CricketParkDto>>(cricketParks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching cricket parks with term {SearchTerm}", searchTerm);
                throw;
            }
        }
    }
} 