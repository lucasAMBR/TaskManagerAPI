using System.Collections;
using System.Security.Claims;
using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers{

    [ApiController]
    [Route("api/project")]
    public class ProjectController : ControllerBase {

        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService){
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAll(){
            return await _projectService.GetAllProjectsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetById(string id){
            return await _projectService.GetProjectByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = "MNG")]
        public async Task<ActionResult<Project>> Create(CreateProjectDTO project)
        {
            var managerIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (managerIdFromToken == null)
            {
                return Unauthorized("Only managers can create a project");
            }

            var created = await _projectService.CreateProjectAsync(managerIdFromToken, project);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "MNG")]
        public async Task<ActionResult<Project>> Update(string id, UpdateProjectDTO project)
        {
            var managerTokenFromBody = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Project foundedProject = await _projectService.GetProjectByIdAsync(id);

            if (managerTokenFromBody == null)
            {
                return Unauthorized("Only managers can update a project");
            }

            if (foundedProject.ManagerId != managerTokenFromBody)
            {
                return Unauthorized("You only can update your projects");
            }

            var updated = await _projectService.UpdateProjectAsync(id, project);

            if (updated == null)
            {
                return NotFound("Project not Found!!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MNG")]
        public async Task<IActionResult> Delete(string id)
        {
            var managerTokenFromBody = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Project foundedProject = await _projectService.GetProjectByIdAsync(id);

            if (managerTokenFromBody == null)
            {
                return Unauthorized("Only managers can delete a project");
            }

            if (foundedProject.ManagerId != managerTokenFromBody)
            {
                return Unauthorized("You only can delete your projects");
            }

            var deleted = await _projectService.DeleteProjectAsync(id);

            if (!deleted)
            {
                return NotFound("Project not found!!");
            }

            return NoContent();
        }

    }
}