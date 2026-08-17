using System.ComponentModel.DataAnnotations;

namespace TaskMenagementAPI.DTOs.ProjectTasks
{
    public class UpdateProjectTaskDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = "";

        public string Description { get; set; } = "";
    }
}
