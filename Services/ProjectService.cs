using Microsoft.EntityFrameworkCore;
using TaskMenagementAPI.Data;
using TaskMenagementAPI.DTOs.Projects;
using TaskMenagementAPI.Enums;
using TaskMenagementAPI.Exceptions;
using TaskMenagementAPI.Models;

namespace TaskMenagementAPI.Services
{
    public class ProjectService
    {
        private readonly AppDbContext _context;

        private readonly ProjectAccessService _projectAccessService;

        public ProjectService(AppDbContext context, ProjectAccessService projectAccessService)
        {
            _context = context;
            _projectAccessService = projectAccessService;
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(int id, int currentUserId)
        {
            var project = await _projectAccessService
                .GetOwnedProjectAsync(id, currentUserId);

            if (project == null)
                return null;

            return ToDto(project);
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto, int currentUserId)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                OwnerId = currentUserId
            };

            var manager = new ProjectMember
            {
                UserId = currentUserId,
                Project = project,
                Role = ProjectRole.Manager
            };

            _context.Projects.Add(project);

            _context.Members.Add(manager);

            await _context.SaveChangesAsync();

            return ToDto(project);
        }

        public async Task<List<ProjectDto>> GetMyProjectsAsync(int currentUserId)
        {
            var projects = await _context.Projects
                .Where(p => p.OwnerId == currentUserId)
                .ToListAsync();  
            
            return projects.Select(ToDto).ToList();
        }

        public async Task<ProjectDto?> UpdateProjectAsync(int id, int currentUserId, UpdateProjectDto dto)
        {
            var project = await _projectAccessService
                .GetOwnedProjectAsync(id, currentUserId);

            if (project == null)
                return null;

            project.Name = dto.Name;
            project.Description = dto.Description;

            await _context.SaveChangesAsync();

            return ToDto(project);
        }

        public async Task<bool> DeleteProjectAsync(int id, int currentUserId)
        {
            var project = await _projectAccessService
                .GetOwnedProjectAsync(id, currentUserId);

            if (project == null)
                return false;

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return true;
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
