using Microsoft.EntityFrameworkCore;
using TaskMenagementAPI.Data;
using TaskMenagementAPI.Enums;
using TaskMenagementAPI.Exceptions;
using TaskMenagementAPI.Models;

namespace TaskMenagementAPI.Services
{
    public class ProjectAccessService
    {
        private readonly AppDbContext _context;

        public ProjectAccessService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetOwnedProjectAsync(int projectId, int currentUserId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return null;

            if (project.OwnerId != currentUserId)
                throw new AccessDeniedException("Access to this project is denied");

            return project;
        }

        public async Task<ProjectMember?> GetProjectMemberAsync(int projectId, int currentUserId)
        {          
            var member = await _context.Members
                .Include(pm => pm.Project)
                .FirstOrDefaultAsync(pm =>
                    pm.ProjectId == projectId &&
                    pm.UserId == currentUserId);

            if (member == null) 
                return null;

            return member;
        }

        public async Task<ProjectMember?> GetProjectManagerAsync(int projectId, int currentUserId)
        {
            var member = await GetProjectMemberAsync(projectId, currentUserId);

            if (member == null)
                return null;

            if (member.Role != ProjectRole.Manager)
                throw new AccessDeniedException("You don't have permission to manage this project");

            return member;
        }
    }
}
