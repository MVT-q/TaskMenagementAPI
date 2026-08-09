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
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(int id)
        {
            var project = await _projectService
                .GetProjectByIdAsync(id, CurrentUserId);

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
        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> UpdateProject(int id, UpdateProjectDto dto)
        {
            var project = await _projectService
                .UpdateProjectAsync(id, CurrentUserId, dto);

            if(project == null)
                return NotFound();

            return Ok(project);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var delete = await _projectService
                .DeleteProjectAsync(id, CurrentUserId);

            if(!delete)
                return NotFound();

            return NoContent();
        }
    }
}
