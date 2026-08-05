using TaskMenagementAPI.Enums;

namespace TaskMenagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string PasswordHash { get; set; } = "";

        public UserRole Role { get; set; } = UserRole.Member;

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
