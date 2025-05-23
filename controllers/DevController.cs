using System.Security.Claims;
using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers{
    [ApiController]
    [Route("api/dev")]
    public class DevController : ControllerBase{

        private readonly IDevService _devService;

        public DevController(IDevService devService){
            _devService = devService;
        }

        /// <summary>
        /// Lista todos os devs no sistema, criado apenas para fins de teste
        /// </summary>
        /// <returns>Lista de devs</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DevResponseDTO>>> GetAll(){
            var devs = await _devService.GetAllDevsAsync();

            var devsListDTO = devs.Select(dev => new DevResponseDTO {
                Id = dev.Id,
                Name = dev.Name,
                Email = dev.Email
            }).ToList();

            return Ok(devsListDTO);
        }

        /// <summary>
        /// Pega os dados de um Dev
        /// </summary>
        /// <param name="id"> id do dev passado pela URL</param>
        /// <returns>um objeto com: id, name, email</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DevResponseDTO>> GetById(string id){
            var dev = await _devService.GetDevByIdAsync(id);

            return Ok(new DevResponseDTO {
                Id = dev.Id,
                Name = dev.Name,
                Email = dev.Email
            }); 
        }

        /// <summary>
        /// Lista todos os devs de uma equipe escolhida
        /// </summary>
        /// <param name="equipId">id da equipe</param>
        /// <returns>retorna uma lista de desenvolvedores, cada um com: id, name, email</returns>
        [HttpGet("equip/{equipId}/all")]
        public async Task<ActionResult<List<Dev>>> GetAllMembersByEquipId(string equipId)
        {
            return await _devService.GetAllMembersByEquipId(equipId);
        }

        [HttpPost]
        public async Task<ActionResult<DevResponseDTO>> Create(CreateDevDTO dev){
            if (await _devService.GetByEmailAsync(dev.Email))
            {
                return Conflict("Email already in use");
             }

            var created = await _devService.CreateDevAsync(dev);

            var responseDTO = new DevResponseDTO {
                Id = created.Id,
                Name = created.Name,
                Email = created.Email
            };

            return CreatedAtAction(nameof(GetById), new {id = created.Id}, responseDTO);
        }

        /// <summary>
        /// Atualiza os dados de um dev
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// </remarks>
        /// <param name="id">id do dev escolhido para sofrer as alterações</param>
        /// <param name="dev">
        /// dados que serão colocados no lugar dos antigos, pode conter:
        /// - name (opcional): novo nome do dev,
        /// - password (opcional): nova senha do dev 
        /// </param>
        /// <returns>Sem Retorno</returns>
        /// <response code = "204">Informações atualizadas com sucesso</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "DEV")]
        public async Task<ActionResult<DevResponseDTO>> Update(string id, UpdateDevDTO dev)
        {
            var devIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (devIdFromToken == null)
            {
                return Unauthorized("You must be logged in to update your account information");
            }

            if (devIdFromToken != id)
            {
                return Forbid("You only can update YOUR account information");
            }

            var update = await _devService.UpdateDevAsync(devIdFromToken, dev);

            if (update == null)
            {
                return NotFound("User not found!!");
            }

            return NoContent();
        }

        /// <summary>
        /// Deleta um dev e todas as suas informações relacionadas, porem matem suas tasks atribuidas com Assignee = null
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// </remarks>
        /// <param name="id">Id do dev a ser removido</param>
        /// <returns>Sem retorno</returns>
        /// <response code = "204">Dev devidamente excluido</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "DEV")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var devIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (devIdFromToken != id)
            {
                return Unauthorized("You can delete only YOUR account");
            }

            var deleted = await _devService.DeleteDevAsync(devIdFromToken);

            if (!deleted)
            {
                return NotFound("User not found!!");
            }

            return NoContent();
        }
    }
}