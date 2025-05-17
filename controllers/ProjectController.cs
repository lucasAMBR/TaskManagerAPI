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
        public async Task<ActionResult<Project>> Update(string id, Project project){
            if(id != project.Id){
                return BadRequest("the URL id doens't match with the id in the request body!!");
            }

            var updated = await _projectService.UpdateProjectAsync(project);

            if(updated == null){
                return NotFound("Project not Found!!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){
            var deleted = await _projectService.DeleteProjectAsync(id);

            if(!deleted){
                return NotFound("Project not found!!");
            }

            return NoContent();
        }

    }
}