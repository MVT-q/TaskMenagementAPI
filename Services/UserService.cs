using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskMenagementAPI.Data;
using TaskMenagementAPI.Enums;
using TaskMenagementAPI.Models;
using TaskMenagementAPI.Settings;

namespace TaskMenagementAPI.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        private readonly AdminSettings _adminSettings;

        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(AppDbContext context, IOptions<AdminSettings> adminSettings, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _adminSettings = adminSettings.Value;
            _passwordHasher = passwordHasher;
        }

        public async Task CreateAdmin()
        {
            var exists = await _context.Users.AnyAsync(u => u.Role == UserRole.Admin);

            if (exists)
                return;

            var admin = new User
            {
                Username = _adminSettings.Username,
                Role = UserRole.Admin
            };

            admin.PasswordHash = _passwordHasher.HashPassword(admin, _adminSettings.Password);

            _context.Add(admin);

            await _context.SaveChangesAsync();
        }
    }
}
