using TaskMenagementAPI.Enums;

namespace TaskMenagementAPI.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public UserRole Role { get; set; }
    }
}
