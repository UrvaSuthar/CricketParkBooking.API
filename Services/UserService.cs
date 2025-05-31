using AutoMapper;
using CricketParkBooking.API.Constants;
using CricketParkBooking.API.DTOs;
using CricketParkBooking.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CricketParkBooking.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            IJwtService jwtService,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            try
            {
                var users = await _userManager.Users.Where(u => u.IsActive).ToListAsync();
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all users");
                throw;
            }
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null || !user.IsActive)
                {
                    return null;
                }
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user with ID {UserId}", id);
                throw;
            }
        }

        public async Task<UserDto> RegisterAsync(CreateUserDto createUserDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(createUserDto.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException(AppConstants.ErrorMessages.DuplicateEmail);
                }

                var user = _mapper.Map<User>(createUserDto);
                user.UserName = createUserDto.Email;
                user.IsActive = true;
                user.CreatedAt = DateTime.UtcNow;

                var result = await _userManager.CreateAsync(user, createUserDto.Password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                // Assign default role
                await _userManager.AddToRoleAsync(user, AppConstants.Roles.User);

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user");
                throw;
            }
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (user == null || !user.IsActive)
                {
                    throw new InvalidOperationException(AppConstants.ErrorMessages.InvalidCredentials);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(AppConstants.ErrorMessages.InvalidCredentials);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtService.GenerateToken(user, roles);

                return new LoginResponseDto
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    Token = token
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user");
                throw;
            }
        }

        public async Task<UserDto> UpdateAsync(string id, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null || !user.IsActive)
                {
                    throw new KeyNotFoundException(AppConstants.ErrorMessages.NotFound);
                }

                _mapper.Map(updateUserDto, user);
                user.UpdatedAt = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(updateUserDto.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, updateUserDto.Password);
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                }

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user {UserId}", id);
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null || !user.IsActive)
                {
                    throw new KeyNotFoundException(AppConstants.ErrorMessages.NotFound);
                }

                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user {UserId}", id);
                throw;
            }
        }
    }
} 