using System.Collections;
using System.Security.Claims;
using DTOs;
using Formatter;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers{

    [ApiController]
    [Route("api/equip")]
    public class EquipController : ControllerBase
    {

        private readonly IEquipService _equipService;

        private readonly IEquipAndDevService _equipAndDevService;

        public EquipController(IEquipService equipService, IEquipAndDevService equipAndDevService)
        {
            _equipService = equipService;
            _equipAndDevService = equipAndDevService;
        }

        /// <summary>
        /// Lista todas as equipes criadas no sistema, criado para fins de teste
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equip>>> GetAll()
        {
            return await _equipService.GetAllEquipsAsync();
        }

        /// <summary>
        /// Pega os dados de uma unica equipe
        /// </summary>
        /// <param name="id">Id da equipe que sera buscada, passado pela URL</param>
        /// <returns>um objeto com: id, leaderId, leaderName, projectId, projectName, departament, description</returns>
        /// <response code="200">Deu tudo certo</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipResponseDTO>> GetById(string id)
        {
            var equipData = await _equipService.GetEquipByIdAsync(id);

            return FormatterHelper.EquipFormater(equipData);
        }

        [HttpGet("project/{projectId}")]
        [Authorize(Roles = "MNG")]
        public async Task<ActionResult<List<Equip>>> GetAllEquipsByProjectId(string projectId)
        {
            var managerIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (managerIdFromToken == null)
            {
                return Unauthorized("You must be logged in to create a team.");
            }

            return await _equipService.GetAllEquipsByProjectId(projectId);
            
        }

        [HttpGet("dev")]
        [Authorize(Roles = "DEV")]
        public async Task<ActionResult<List<Equip>>> GetAllEquipsByDevId()
        {
            var managerIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (managerIdFromToken == null)
            {
                return Unauthorized("You must be logged in to create a team.");
            }

            return await _equipService.GetAllEquipsByDevId(managerIdFromToken);
            
        }

        /// <summary>
        /// Cria um nova equipe dentro de um projeto
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// </remarks>
        /// <param name="equip">
        /// Um objeto com os seguintes dados: 
        /// - leaderId : id do dev escolhido para liderar essa equipe
        /// - projectId : id do projeto que essa equipe pertence
        /// - departament: departamento dessa equipe, backend, frontend, design, UI/UX e afins
        /// - description: detalhes do que essa equipe vai fazer, seu objetivo
        /// </param>
        /// <returns>dados da equipe cadastrada: id, leaderId, leaderName, projectId, projectName, departament, description</returns>
        [HttpPost]
        [Authorize(Roles = "MNG")]
        public async Task<ActionResult<EquipResponseDTO>> Create(CreateEquipDTO equip)
        {
            var managerIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (managerIdFromToken == null)
            {
                return Unauthorized("You must be logged in to create a team.");
            }

            var created = await _equipService.CreateEquipAsync(managerIdFromToken, equip);

            var addResult = await _equipAndDevService.AddDevToEquip(created.Id, equip.LeaderId);

            if (!addResult)
            {
                return BadRequest("We encountered an error while adding this member to the team.");
            }

            return Ok(FormatterHelper.EquipFormater(created));
        }

        /// <summary>
        /// Altera informações de um equipe
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// Apenas managers, podem editar suas PROPRIAS equipes
        /// </remarks>
        /// <param name="id">id da equipe que sera modificada, passado pela URL</param>
        /// <param name="equip">
        /// dados que substituirão os dados antigos, pode conter: 
        /// - leaderId (opcional): id do novo lider da equipe
        /// - departament (opcional): novo departamento da equipe
        /// - description (opcional): nova descrição da equipe
        /// </param>
        /// <returns>sem retorno</returns>
        /// <response code="204">Alterou com sucesso</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "MNG")]
        public async Task<ActionResult<Equip>> Update(string id, UpdateEquipDTO equip)
        {
            var managerIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (managerIdFromToken == null)
            {
                return Unauthorized("You must be logged in to update team information.");
            }

            var updated = await _equipService.UpdateEquipAsync(managerIdFromToken, id, equip);

            if (updated == null)
            {
                return NotFound("The requested team could not be found.");
            }

            return NoContent();
        }

        /// <summary>
        /// Remove um equipe do sistema
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// </remarks>
        /// <param name="id">id da equipe a ser removida juntamente com seus dados relacionados</param>
        /// <returns>sem retorno</returns>
        /// <response code="204">Tudo foi removido com sucesso</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "MNG")]
        public async Task<IActionResult> Delete(string id)
        {
            var managerIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (managerIdFromToken == null)
            {
                return Unauthorized("You must be logged in to delete a team.");
            }

            var deleted = await _equipService.DeleteEquipAsync(managerIdFromToken, id);

            if (!deleted)
            {
                return NotFound("The requested team could not be found.");
            }

            return NoContent();
        }

        /// <summary>
        /// Adiciona um dev em uma equipe
        /// Exemplo de URL: base:porta/api/equip/{id da equipe}/add/{id do dev}
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// Só pode ser utilizado pelo manager do projeto que essa equipe pertence, ou pelo dev lider dessa equipe
        /// </remarks>
        /// <param name="equipId">id da equipe</param>
        /// <param name="devId">id do dev que será adicionado</param>
        /// <returns></returns>
        [HttpPost("{equipId}/add/{devId}")]
        [Authorize(Roles = "MNG,DEV")]
        public async Task<IActionResult> AddDevToEquip(string equipId, string devId)
        {
            var equip = await _equipService.GetEquipByIdAsync(equipId);

            if (equip.Project == null)
            {
                return BadRequest("Error: The specified team was not found.");
            }

            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (User.IsInRole("MNG"))
            {
                if (equip.Project.ManagerId != userIdFromToken)
                {
                    return Forbid("Member addition is restricted to project managers.");
                }
            }

            if (User.IsInRole("DEV"))
            {
                if (equip.LeaderId != userIdFromToken)
                {
                    return Forbid("Member addition is restricted to team leaders.");
                }
            }

            var addResult = await _equipAndDevService.AddDevToEquip(equipId, devId);

            if (!addResult)
            {
                return BadRequest("Error: The provided Team/Device ID is not valid.");
            }

            return NoContent();
        }

        /// <summary>
        /// Remove um dev de uma determinada equipe
        /// exemlo de URL: base:porta/api/equip/{id da equipe}/remove/{id do dev}
        /// </summary>
        /// <param name="equipId">id da equipe</param>
        /// <param name="devId">id do dev que sera removido da equipe</param>
        /// <returns>sem retorno</returns>
        /// <response code="204">Removido com sucesso</response>
        [HttpDelete("{equipId}/remove/{devId}")]
        [Authorize(Roles = "MNG,DEV")]
        public async Task<IActionResult> RemoveDevFromEquip(string equipId, string devId)
        {
            var equip = await _equipService.GetEquipByIdAsync(equipId);

            if (equip.Project == null)
            {
                return BadRequest("Error: Cannot add members to non-existent team.");
            }

            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (User.IsInRole("MNG"))
            {
                if (equip.Project.ManagerId != userIdFromToken)
                {
                    return Forbid("Only managers can remove team members.");
                }
            }

            if (User.IsInRole("DEV"))
            {
                if (equip.LeaderId != userIdFromToken)
                {
                    return Forbid("Member removal is restricted to team leaders.");
                }
            }
            
            var removeResult = await _equipAndDevService.RemoveDevFromEquip(equipId, devId);

            if (!removeResult)
            {
                return BadRequest("Invalid Team ID or Device ID.");
            }

            return NoContent();
        }
    }
}