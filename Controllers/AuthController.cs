using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExpensesApp.API.Data;
using ExpensesApp.API.Dtos;
using ExpensesApp.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExpensesApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class AuthController(AppDbContext appDbContext, PasswordHasher<User> passwordHasher) : ControllerBase
    {
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUserDto loginUserDto)
        {
            var user = appDbContext.Users.FirstOrDefault(n => n.Email == loginUserDto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var result = passwordHasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrWhiteSpace(registerUserDto.Email) || string.IsNullOrWhiteSpace(registerUserDto.Password))
                    return BadRequest("Invalid registration data");

                if (appDbContext.Users.Any(n => n.Email == registerUserDto.Email))
                    return BadRequest("This email address is already taken");

                var newUser = new User
                {
                    Email = registerUserDto.Email,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                newUser.Password = passwordHasher.HashPassword(newUser, registerUserDto.Password);

                appDbContext.Users.Add(newUser);
                appDbContext.SaveChanges();

                var token = GenerateJwtToken(newUser);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
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