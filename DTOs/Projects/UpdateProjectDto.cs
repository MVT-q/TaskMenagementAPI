using System.ComponentModel.DataAnnotations;

namespace TaskMenagementAPI.DTOs.Projects
{
    public class UpdateProjectDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";
    }
}
