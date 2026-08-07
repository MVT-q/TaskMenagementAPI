using Microsoft.EntityFrameworkCore;
using TaskMenagementAPI.Data;
using TaskMenagementAPI.DTOs.Projects;
using TaskMenagementAPI.DTOs.ProjectTasks;
using TaskMenagementAPI.Exceptions;
using TaskMenagementAPI.Models;

namespace TaskMenagementAPI.Services
{
    public class ProjectTaskService
    {
        private readonly AppDbContext _context;

        public ProjectTaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectTaskDto?> GetTaskByIdAsync(int projectId, int currentUserId, int taskId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return null;

            if (project.OwnerId != currentUserId)
                throw new AccessDeniedException("Access to this project is denied");

            var task = await _context.Tasks
                .FirstOrDefaultAsync(t =>
                    t.Id == taskId &&
                    t.ProjectId == projectId);

            if (task == null) 
                return null;

            return ToDto(task);
        }

        public async Task<ProjectTaskDto?> CreateAsync(int projectId, CreateProjectTaskDto dto, int currentUserId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return null;

            if(project.OwnerId != currentUserId) 
                throw new AccessDeniedException("Access to this project is denied");

            var task = new ProjectTask
            {
                Title = dto.Title,
                Description = dto.Description,
                ProjectId = projectId
            };

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return ToDto(task);
        }

        public async Task<List<ProjectTaskDto>?> GetProjectTasksAsync(int projectId, int currentUserId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return null;

            if (project.OwnerId != currentUserId)
                throw new AccessDeniedException("Access to this project is denied");

            var tasks = await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();

            return tasks.Select(ToDto).ToList();
        }

        private static ProjectTaskDto ToDto(ProjectTask task)
        {
            return new ProjectTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status
            };
        }
    }
}
