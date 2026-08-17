using System.ComponentModel.DataAnnotations;

namespace TaskMenagementAPI.DTOs.ProjectTasks
{
    public class CreateProjectTaskDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime? DueDate { get; set; }
    }
}
