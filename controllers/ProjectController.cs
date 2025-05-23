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

        /// <summary>
        /// Lista todos os projetos criados no sistema, criado a fim de testes
        /// </summary>
        /// <returns>Uma lista com todos os usuarios</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAll(){
            return await _projectService.GetAllProjectsAsync();
        }

        /// <summary>
        /// Pega os dados de um unico projeto
        /// </summary>
        /// <param name="id">id do projeto, passado pela URL</param>
        /// <returns>dados de um projeto contendo: id, name, description, goals, managerId</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetById(string id){
            return await _projectService.GetProjectByIdAsync(id);
        }

        /// <summary>
        /// Cria um projeto
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// </remarks>
        /// <param name="project">
        /// Um objeto com os dados do projeto contendo:
        /// - name : Nome do projeto a ser desenvolvido,
        /// - description : Descrição da proposta do projeto
        /// - goals : Metas propostas para o projeto
        /// </param>
        /// <returns>Retorna um objeto com os seguintes dados: id, name, description, goals, managerId</returns>
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

        /// <summary>
        /// Altera as informações de um projeto
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// OBS: o id do usuario dentro das claims do token tem que ser o mesmo do managerId desse projeto para poder alterar os dados
        /// </remarks>
        /// <param name="id">Id do projeto a ser alterado (passado pela URL)</param>
        /// <param name="project">
        /// Os novos dados referentes ao projeto, podendo conter:
        /// - name (opcional): novo nome do projeto,
        /// - description (opcional): nova descrição do projeto,
        /// - goals (opcional): Novas metas do projeto
        /// </param>
        /// <returns>Sem retorno</returns>
        /// <response code = "204">Dados alterados com sucesso</response>
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

        /// <summary>
        /// Remove um projeto do sistema
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// OBS: o usuario logado pode remover apenas os proprios projetos
        /// </remarks>
        /// <param name="id">Id do projeto que sera removido</param>
        /// <returns>Sem retorno</returns>
        /// <response code="204">Tudo foi removido com sucesso</response>
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