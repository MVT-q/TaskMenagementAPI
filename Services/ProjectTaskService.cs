using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;
using TaskMenagementAPI.Data;
using TaskMenagementAPI.DTOs.Projects;
using TaskMenagementAPI.DTOs.ProjectTasks;
using TaskMenagementAPI.Exceptions;
using TaskMenagementAPI.Models;
using TaskMenagementAPI.Enums;

namespace TaskMenagementAPI.Services
{
    public class ProjectTaskService
    {
        private readonly AppDbContext _context;

        private readonly ProjectAccessService _projectAccessService;

        private readonly INotificationService _notificationService;

        public ProjectTaskService(AppDbContext context, ProjectAccessService projectAccessService, INotificationService notificationService)
        {
            _context = context;
            _projectAccessService = projectAccessService;
            _notificationService = notificationService;
        }

        public async Task<ProjectTaskDto?> GetTaskByIdAsync(int projectId, int currentUserId, int taskId)
        {
            if (await _projectAccessService
                .GetProjectMemberAsync(projectId, currentUserId) == null)
                return null;

            var task = await GetTaskForProjectAsync(projectId, taskId);

            if (task == null) 
                return null;

            return ToDto(task);
        }

        public async Task<ProjectTaskDto?> CreateTaskAsync(int projectId, CreateProjectTaskDto dto, int currentUserId)
        {
            if (await _projectAccessService
                .GetProjectManagerAsync(projectId, currentUserId) == null)
                return null;

            if (dto.DueDate < DateTime.UtcNow)
                throw new InvalidDueDateException("Date cannot be earlier than current date");

            var task = new ProjectTask
            {
                Title = dto.Title,
                Description = dto.Description,
                ProjectId = projectId,
                DueDate = dto.DueDate
            };

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return ToDto(task);
        }

        public async Task<List<ProjectTaskDto>?> GetProjectTasksAsync(int projectId, int currentUserId)
        {
            if (await _projectAccessService
                .GetProjectMemberAsync(projectId, currentUserId) == null)
                return null;

            var tasks = await _context.Tasks
                .Include(t => t.Assignee)
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();

            return tasks.Select(ToDto).ToList();
        }

        public async Task<ProjectTaskDto?> UpdateProjectTaskAsync(int projectId, int currentUserId, int taskId, UpdateProjectTaskDto dto)
        {
            if (await _projectAccessService
                .GetProjectManagerAsync(projectId, currentUserId) == null)
                return null;

            var task = await GetTaskForProjectAsync(projectId, taskId);

            if (task == null)
                return null;

            task.Title = dto.Title;
            task.Description = dto.Description;

            await _context.SaveChangesAsync();

            return ToDto(task);
        }

        public async Task<bool> DeleteProjectTaskAsync(int projectId, int currentUserId, int taskId)
        {
            if (await _projectAccessService
                .GetProjectManagerAsync(projectId, currentUserId) == null)
                return false;

            var task = await GetTaskForProjectAsync(projectId, taskId);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ProjectTaskDto?> UpdateTaskStatusAsync(int projectId, int taskId, int currentUserId, UpdateProjectTaskStatusDto dto)
        {
            var currentMember = await _projectAccessService
                .GetProjectMemberAsync(projectId, currentUserId);

            if (currentMember == null)
                return null;

            var task = await GetTaskForProjectAsync(projectId, taskId);

            if (task == null)
                return null;

            if (!Enum.IsDefined(dto.Status))
                throw new InvalidProjectTaskStatusException("Invalid project task status");

            if (currentMember.Role == ProjectRole.Manager)
                task.Status = dto.Status;

            else if(task.AssigneedId == currentUserId)              
                task.Status = dto.Status;

            else
                throw new AccessDeniedException("You don't have permission to change this task");

            await _context.SaveChangesAsync();

            return ToDto(task);
        }

        public async Task<ProjectTaskDto?> UpdateTaskPriorityAsync(int projectId, int taskId, int currentUserId, UpdateProjectTaskPriorityDto dto)
        {
            if (await _projectAccessService
                .GetProjectManagerAsync(projectId, currentUserId) == null)
                return null;

            var task = await GetTaskForProjectAsync(projectId, taskId);

            if (task == null)
                return null;

            if (!Enum.IsDefined(dto.Priority))
                throw new InvalidProjectTaskPriorityException("Invalid project task priority");

            task.Priority = dto.Priority;

            await _context.SaveChangesAsync();

            return ToDto(task);
        }

        public async Task<ProjectTaskDto?> UpdateTaskDueDateAsync(int projectId, int taskId, int currentUserId, UpdateProjectTaskDueDateDto dto)
        {
            if (await _projectAccessService
                .GetProjectManagerAsync(projectId, currentUserId) == null)
                return null;

            var task = await GetTaskForProjectAsync(projectId, taskId);

            if (task == null)
                return null;

            if (dto.DueDate < DateTime.UtcNow)
                throw new InvalidDueDateException("Date cannot be earlier than current date");

            task.DueDate = dto.DueDate;

            await _context.SaveChangesAsync();

            return ToDto(task);
        }

        public async Task<ProjectTaskDto?> AssignTaskAsync(int projectId, int taskId, int currentUserId, UpdateProjectTaskAssigneeDto dto)
        {
            if (await _projectAccessService
                .GetProjectManagerAsync(projectId, currentUserId) == null)
                return null;

            var task = await GetTaskForProjectAsync(projectId, taskId);

            if (task == null) 
                return null;         

            if(dto.UserId == null)
            {
                task.AssigneedId = null;
            }
            else
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == dto.UserId);

                if (user == null) 
                    return null;

                var userId = dto.UserId.Value;

                if (await _projectAccessService
                    .GetProjectMemberAsync(projectId, userId) == null)
                    return null;

                task.AssigneedId = user.Id;
                task.Assignee = user;
            }

            await _context.SaveChangesAsync();

            if (dto.UserId != null)
            {
                await _notificationService
                    .NotifyTaskAssignedAsync(dto.UserId.Value, task.Id);
            }

            return ToDto(task);
        }

        private async Task<ProjectTask?> GetTaskForProjectAsync(int projectId, int taskId)
        {
            return await _context.Tasks
                .Include(t => t.Assignee)
                .FirstOrDefaultAsync(t =>
                    t.Id == taskId &&
                    t.ProjectId == projectId);
        }

        private static ProjectTaskDto ToDto(ProjectTask task)
        {
            return new ProjectTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                AssigneeId = task.AssigneedId,
                AssigneeUsername = task.Assignee?.Username
            };
        }
    }
}
