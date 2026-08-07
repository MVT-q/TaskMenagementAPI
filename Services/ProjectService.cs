using Microsoft.EntityFrameworkCore;
using TaskMenagementAPI.Data;
using TaskMenagementAPI.DTOs.Projects;
using TaskMenagementAPI.Exceptions;
using TaskMenagementAPI.Models;

namespace TaskMenagementAPI.Services
{
    public class ProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(int id, int currentUserId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return null;

            if (project.OwnerId != currentUserId)
                throw new AccessDeniedException("Access to this project is denied");

            return ToDto(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto, int currentUserId)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                OwnerId = currentUserId
            };

            await _context.Projects.AddAsync(project);

            await _context.SaveChangesAsync();

            return ToDto(project);
        }

        public async Task<List<ProjectDto>> GetMyProjectsAsync(int currentUserId)
        {
            var projects = await _context.Projects
                .Where(p => p.OwnerId == currentUserId).ToListAsync();  
            
            return projects.Select(ToDto).ToList();
        }

        private static ProjectDto ToDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
        }
    }
}
