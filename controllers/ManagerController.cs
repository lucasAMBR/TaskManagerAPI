using System.Security.Claims;
using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Controllers{
    
    [ApiController]
    [Route("api/manager")]
    public class ManagerController : ControllerBase{

        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService){
            _managerService = managerService;
        }
        
        /// <summary>
        /// Retorna todos os managers cadastrados no sistema, não precisa que nada seja passado mas não sera usado pelo frontend, criei so para fins de testes
        /// </summary>
        /// <returns>Lista de todos os managers</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManagerResponseDTO>>> GetAll(){
            var managers = await _managerService.GetAllManagersAsync();

            var managersListDTO = managers.Select(manager =>new ManagerResponseDTO{
                Id = manager.Id,
                Name = manager.Name,
                Email = manager.Email
            }).ToList();

            return Ok(managersListDTO);
        }

        /// <summary>
        /// Busca um manager pelo id e retorna seus dados
        /// </summary>
        /// <param name="id">Id do manager</param>
        /// <returns>Um objeto com o id, name, email</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ManagerResponseDTO>> GetById(string id){
            var manager = await _managerService.GetManagerByIdAsync(id);

            return Ok(new ManagerResponseDTO {
                Id = manager.Id,
                Name = manager.Name,
                Email = manager.Email
            });
        }

        /// <summary>
        /// Registra um novo manager no sistema
        /// </summary>
        /// <param name="manager">
        /// Um objeto com os seguintes dados do manager:
        /// - name : O nome do manager,
        /// - email : O email do manager,
        /// - password : A senha do usuario
        /// </param>
        /// <returns>O objeto do manager cadastrado: id, name, email</returns>
        [HttpPost]
        public async Task<ActionResult<ManagerResponseDTO>> Create(CreateManagerDTO manager){

            if (await _managerService.VerifyEmail(manager.Email))
            {
                return Conflict("Email Already in use");
            }

            var created = await _managerService.CreateManagerAsync(manager);

            var responseDTO = new ManagerResponseDTO {
                Id = created.Id,
                Name = created.Name,
                Email = created.Email
            };

            return CreatedAtAction(nameof(GetById), new {id = created.Id}, responseDTO);
        }

        /// <summary>
        /// Atualiza os dados de um manager
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// </remarks>
        /// <param name="id">id do manager que se deseja alterar (passado pela URL)</param>
        /// <param name="manager">
        /// Um objeto com os possiveis seguintes dados: 
        /// - name (opcional) : Novo nome do manager
        /// - password (opcional) : Nova senha desse manager
        /// </param>
        /// <returns>Sem retorno</returns>
        /// <response code = "204">Informações atualizadas com sucesso</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "MNG")]
        public async Task<IActionResult> Update(string id, UpdateManagerDTO manager)
        {
            var managerIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (id != managerIdFromToken)
            {
                return BadRequest("you cannot update someone information!!");
            }

            var updated = await _managerService.UpdateManagerAsync(id, manager);

            if (updated == null)
            {
                return NotFound("User not Found!!");
            }

            return NoContent();
        }

        /// <summary>
        /// Deleta a conta de um manager e todos os seus dados relacionados
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// </remarks>
        /// <param name="id">id do manager</param>
        /// <returns>Sem retorno</returns>
        /// <response code = "204">Informações removidas com sucesso</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "MNG")]
        public async Task<IActionResult> Delete(string id)
        {
            var managerIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (managerIdFromToken != id)
            {
                Forbid("You Cannot delete someone account");
            }

            var deleted = await _managerService.DeleteManagerAsync(id);

            if (!deleted)
            {
                return NotFound("User not found!!");
            }

            return NoContent();
        }

    }
}