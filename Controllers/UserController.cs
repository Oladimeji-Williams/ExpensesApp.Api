using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using ExpensesApp.API.Data;
using ExpensesApp.API.Dtos;
using ExpensesApp.API.Models;

namespace ExpensesApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("GetProfile")]
        public IActionResult GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user ID in token.");
            }

            var user = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.PhoneNumber,
                    u.Address,
                    u.ProfileImagePath,
                    u.CreatedAt,
                    u.UpdatedAt
                })
                .FirstOrDefault();

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [Authorize]
        [HttpPut("UpdateProfileWithImage")]
        public async Task<IActionResult> UpdateProfileWithImage([FromForm] UserProfileImageDto model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user ID in token.");
            }

            var user = _context.Users.Find(userId);
            if (user == null)
                return NotFound("User not found.");

            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
            user.Address = model.Address ?? user.Address;
            user.UpdatedAt = DateTime.UtcNow;

            // 📷 Handle profile image upload
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.ProfileImage.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                // ✅ Save just 'profiles/{fileName}' so the frontend can construct /uploads/profiles/...
                user.ProfileImagePath = $"profiles/{fileName}";
            }

            _context.SaveChanges();

            return Ok(new
            {
                Message = "Profile updated successfully",
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Address,
                user.ProfileImagePath,
                user.UpdatedAt
            });
        }

    }
}