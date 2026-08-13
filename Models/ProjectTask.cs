using TaskMenagementAPI.Enums;

namespace TaskMenagementAPI.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public int ProjectId { get; set; }

        public Project Project { get; set; } = null!;

        public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Todo;

        public ProjectTaskPriority Priority { get; set; } = ProjectTaskPriority.Medium;

        public DateTime? DueDate { get; set; }

        public int? AssigneedId { get; set; }

        public User? Assignee { get; set; }
    }
}
