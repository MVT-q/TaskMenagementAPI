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

            return ToProjectDto(project);
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

            return ToProjectDto(project);
        }

        public async Task<List<ProjectDto>> GetMyProjectsAsync(int currentUserId)
        {
            var projects = await _context.Projects
                .Where(p => p.OwnerId == currentUserId)
                .ToListAsync();  
            
            return projects.Select(ToProjectDto).ToList();
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

            return ToProjectDto(project);
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

        public async Task<List<ProjectMemberDto>?> GetProjectMembersAsync(int id, int currentUserId)
        {
            var project = await _projectAccessService
                .GetOwnedProjectAsync(id, currentUserId);

            if (project == null)
                return null;

            var members = await _context.Members
                .Include(pm => pm.User)
                .Where(pm => pm.ProjectId == id)
                .ToListAsync();

            if (members == null)
                return null;

            return members.Select(ToMemberDto).ToList();
        }

        public async Task<ProjectMemberDto?> GetMemberByIdAsync(int id, int currentUserId, int userId)
        {
            var project = await _projectAccessService
                .GetOwnedProjectAsync(id, currentUserId);

            if (project == null)
                return null;

            var member = await _context.Members
                .Include(pm => pm.User)
                .FirstOrDefaultAsync(pm =>
                    pm.ProjectId == id &&
                    pm.UserId == userId);

            if (member == null)
                return null;

            return ToMemberDto(member);
        }

        public async Task<ProjectMemberDto?> AddProjectMemberAsync(int id, int currentUserId, AddProjectMemberDto dto)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return null;

            var currentMember = await _projectAccessService
                .GetProjectMemberAsync(id, currentUserId);

            if (currentMember == null)
                return null;

            if (currentMember.Role != ProjectRole.Manager)
                throw new AccessDeniedException("You don't have permission to manage this project");

            var alreadyMember = await _context.Members
                .AnyAsync(pm =>
                    pm.ProjectId == id &&
                    pm.UserId == dto.UserId);

            if (alreadyMember)
                throw new UserAlreadyExistsException("This user already in this project");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user == null)
                return null;

            var member = new ProjectMember
            {
                ProjectId = project.Id,
                UserId = user.Id,
                User = user,
                Role = ProjectRole.Member
            };

            _context.Members.Add(member);

            await _context.SaveChangesAsync();

            return ToMemberDto(member);
        }

        public async Task<bool> DeleteMemberAsync(int id, int currentUserId, int userId)
        {
            if (currentUserId == userId)
                throw new CannotDeleteYourselfException("You cannot delete yourself from project");

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return false;

            var currentMember = await _projectAccessService
                .GetProjectMemberAsync(id, currentUserId);

            if (currentMember == null)
                return false;

            if (currentMember.Role != ProjectRole.Manager)
                throw new AccessDeniedException("You don't have permission to manage this project");

            var member = await _context.Members
                .Include(pm => pm.User)
                .FirstOrDefaultAsync(pm =>
                    pm.ProjectId == id &&
                    pm.UserId == userId);

            if (member == null)
                return false;           

            _context.Members.Remove(member);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ProjectMemberDto?> ChangeProjectMemberRoleAsync(int id, int currentUserId, int userId, UpdateProjectMemberRoleDto dto)
        {
            if (currentUserId == userId)
                throw new CannotChangeOwnRoleException("You cannot change your own role");

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return null;

            var currentMember = await _projectAccessService
                .GetProjectMemberAsync(id, currentUserId);

            if (currentMember == null)
                return null;

            if (currentMember.Role != ProjectRole.Manager)
                throw new AccessDeniedException("You don't have permission to manage this project");

            var member = await _context.Members
                .Include(pm => pm.User)
                .FirstOrDefaultAsync(pm =>
                    pm.ProjectId == id &&
                    pm.UserId == userId);

            if (member == null)
                return null;

            member.Role = dto.Role;

            await _context.SaveChangesAsync();

            return ToMemberDto(member);
        }

        private static ProjectDto ToProjectDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
        }

        private static ProjectMemberDto ToMemberDto(ProjectMember member)
        {
            return new ProjectMemberDto
            {
                UserId = member.UserId,
                Username = member.User.Username,
                Role = member.Role
            };
        }
    }
}
