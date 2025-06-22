using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ExpensesApp.API.Data;
using ExpensesApp.API.Dtos;
using ExpensesApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExpensesApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(AppDbContext appDbContext, PasswordHasher<User> passwordHasher)
        {
            _appDbContext = appDbContext;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                var user = _appDbContext.Users.FirstOrDefault(n => n.Email == loginUserDto.Email);
                if (user == null)
                {
                    return Unauthorized("Invalid credentials");
                }

                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    return Unauthorized("Invalid credentials");
                }

                var token = GenerateJwtToken(user);
                return Ok(new AuthResponseDto(token));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrWhiteSpace(registerUserDto.Email) || string.IsNullOrWhiteSpace(registerUserDto.Password))
                    return BadRequest("Invalid registration data");

                if (_appDbContext.Users.Any(n => n.Email == registerUserDto.Email))
                    return BadRequest("This email address is already taken");

                var newUser = new User
                {
                    Email = registerUserDto.Email,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                newUser.PasswordHash = _passwordHasher.HashPassword(newUser, registerUserDto.Password);

                _appDbContext.Users.Add(newUser);
                _appDbContext.SaveChanges();

                var token = GenerateJwtToken(newUser);
                return Ok(new AuthResponseDto(token));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto model)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized("Invalid user ID in token.");
                }

                var user = _appDbContext.Users.Find(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.CurrentPassword);
                if (result == PasswordVerificationResult.Failed)
                {
                    return BadRequest("Current password is incorrect.");
                }

                if (string.IsNullOrWhiteSpace(model.NewPassword) || model.NewPassword.Length < 6)
                {
                    return BadRequest("New password must be at least 6 characters.");
                }

                user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                _appDbContext.SaveChanges();

                return Ok(new { Message = "Password changed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-secure-secret-key-32-chars-long"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "dotnethow.net",
                audience: "dotnethow.net",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
