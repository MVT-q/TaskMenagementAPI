using System.ComponentModel.DataAnnotations;

namespace TaskMenagementAPI.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [MinLength(4)]
        [StringLength(16)]
        public string Username { get; set; } = "";

        [Required]
        [MinLength(6)]
        [StringLength(16)]
        public string Password { get; set; } = "";
    }
}
