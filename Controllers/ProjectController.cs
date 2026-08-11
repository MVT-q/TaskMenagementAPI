using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMenagementAPI.DTOs.Projects;
using TaskMenagementAPI.Models;
using TaskMenagementAPI.Services;

namespace TaskMenagementAPI.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : BaseController
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [Authorize]
        [HttpGet("{projectId}")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(int projectId)
        {
            var project = await _projectService
                .GetProjectByIdAsync(projectId, CurrentUserId);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto dto)
        {
            var project = await _projectService
                .CreateProjectAsync(dto, CurrentUserId);

            return CreatedAtAction(
                nameof(GetProjectById),
                new { id = project.Id },
                project);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetMyProjects()
        {
            var project = await _projectService
                .GetMyProjectsAsync(CurrentUserId);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [Authorize]
        [HttpPut("{projectId}")]
        public async Task<ActionResult<ProjectDto>> UpdateProject(int projectId, UpdateProjectDto dto)
        {
            var project = await _projectService
                .UpdateProjectAsync(projectId, CurrentUserId, dto);

            if(project == null)
                return NotFound();

            return Ok(project);
        }

        [Authorize]
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var delete = await _projectService
                .DeleteProjectAsync(projectId, CurrentUserId);

            if(!delete)
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpGet("{projectId}/members")]
        public async Task<ActionResult<IEnumerable<ProjectMemberDto>>> GetProjectMembers(int projectId)
        {
            var members = await _projectService
                .GetProjectMembersAsync(projectId, CurrentUserId);

            if (members == null)
                return NotFound();

            return Ok(members);
        }

        [Authorize]
        [HttpGet("{projectId}/members/{userId}")]
        public async Task<ActionResult<ProjectMemberDto>> GetMemberById(int projectId, int userId)
        {
            var member = await _projectService
                .GetMemberByIdAsync(projectId, CurrentUserId, userId);

            if (member == null)
                return NotFound();

            return Ok(member);
        }

        [Authorize]
        [HttpPost("{projectId}/members")]
        public async Task<ActionResult<ProjectMemberDto>> AddProjectMember(int projectId, AddProjectMemberDto dto)
        {
            var member = await _projectService
                .AddProjectMemberAsync(projectId, CurrentUserId, dto);

            if (member == null)
                return NotFound();

            return CreatedAtAction(
                nameof(GetMemberById),
                new
                {
                    projectId,
                    userId = member.UserId
                },
                member);
        }
    }
}
