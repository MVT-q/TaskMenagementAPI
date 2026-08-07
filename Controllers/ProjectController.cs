using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMenagementAPI.DTOs.Projects;
using TaskMenagementAPI.Models;
using TaskMenagementAPI.Services;

namespace TaskMenagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var project = await _projectService.GetProjectByIdAsync(id, CurrentUserId);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto dto)
        {
            var project = await _projectService.CreateAsync(dto, CurrentUserId);

            return CreatedAtAction(
                nameof(GetProjectById),
                new { id = project.Id },
                project);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetMyProjects()
        {
            return Ok(await _projectService.GetMyProjectsAsync(CurrentUserId));
        }
    }
}
