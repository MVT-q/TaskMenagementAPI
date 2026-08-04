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

        private readonly OwnerSettings _ownerSettings;

        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(AppDbContext context, IOptions<OwnerSettings> ownerSettings, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _ownerSettings = ownerSettings.Value;
            _passwordHasher = passwordHasher;
        }

        public async Task CreateOwner()
        {
            var exists = await _context.Users.AnyAsync(u => u.Role == UserRole.Owner);

            if (exists)
                return;

            var owner = new User
            {
                Username = _ownerSettings.Username,
                Role = UserRole.Owner
            };

            owner.PasswordHash = _passwordHasher.HashPassword(owner, _ownerSettings.Password);

            _context.Add(owner);

            await _context.SaveChangesAsync();
        }
    }
}
