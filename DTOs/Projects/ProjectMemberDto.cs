using TaskMenagementAPI.Enums;

namespace TaskMenagementAPI.DTOs.Projects
{
    public class ProjectMemberDto
    {
        public int UserId { get; set; }

        public string Username { get; set; } = "";

        public ProjectRole Role { get; set; }
    }
}
