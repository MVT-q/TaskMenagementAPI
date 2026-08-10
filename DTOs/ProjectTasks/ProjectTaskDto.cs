using TaskMenagementAPI.Enums;

namespace TaskMenagementAPI.DTOs.ProjectTasks
{
    public class ProjectTaskDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public ProjectTaskStatus Status { get; set; }

        public ProjectTaskPriority Priority { get; set; }
    }
}
