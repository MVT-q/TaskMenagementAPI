using Microsoft.EntityFrameworkCore;
using TaskMenagementAPI.Data;
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
                .FirstOrDefaultAsync(pm =>
                    pm.ProjectId == projectId &&
                    pm.UserId == currentUserId);

            return member;
        }
    }
}
