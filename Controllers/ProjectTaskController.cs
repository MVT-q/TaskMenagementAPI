using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMenagementAPI.DTOs.Projects;
using TaskMenagementAPI.DTOs.ProjectTasks;
using TaskMenagementAPI.Models;
using TaskMenagementAPI.Services;

namespace TaskMenagementAPI.Controllers
{
    [ApiController]
    [Route("projects/{projectId}/tasks")]
    public class ProjectTaskController : BaseController
    {
        private readonly ProjectTaskService _projectTaskService;

        public ProjectTaskController(ProjectTaskService projectTaskService)
        {
            _projectTaskService = projectTaskService;
        }

        [Authorize]
        [HttpGet("{taskId}")]
        public async Task<ActionResult<ProjectTaskDto>> GetTaskById(int projectId, int taskId)
        {
            var task = await _projectTaskService.GetTaskByIdAsync(projectId, CurrentUserId, taskId);

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProjectTaskDto>> CreateTask(int projectId, CreateProjectTaskDto dto)
        {
            var task = await _projectTaskService.CreateTaskAsync(projectId, dto, CurrentUserId);

            if(task == null)
                return NotFound();

            return CreatedAtAction(
                nameof(GetTaskById),
                new
                {
                    projectId,
                    taskId = task.Id
                },
                task);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTaskDto>>> GetProjectTasks(int projectId)
        {
            return Ok(await _projectTaskService.GetProjectTasksAsync(projectId, CurrentUserId));
        }

        [Authorize]
        [HttpPut("{taskId}")]
        public async Task<ActionResult<ProjectTaskDto>> UpdateProjectTask(int projectId, int taskId, UpdateProjectTaskDto dto)
        {
            var task = await _projectTaskService.UpdateProjectTaskAsync(projectId, CurrentUserId, taskId, dto);

            if(task == null)
                return NotFound();

            return Ok(task);
        }

        [Authorize]
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteProjectTask(int projectId, int taskId)
        {
            var delete = await _projectTaskService.DeleteProjectTaskAsync(projectId, CurrentUserId, taskId);

            if (!delete)
                return NotFound();

            return NoContent();
        }
    }
}
