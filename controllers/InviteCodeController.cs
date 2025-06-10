using System.Security.Claims;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/invitecode")]
    public class InviteCodeController : ControllerBase
    {

        private readonly IInviteCodeService _inviteCodeService;

        public InviteCodeController(IInviteCodeService inviteCodeService)
        {
            _inviteCodeService = inviteCodeService;
        }

        /// <summary>
        /// Gera um codigo de convite aleatorio que pode ser usado para um dev entrar em alguma equipe
        /// exemplo de URL: base:porta/api/invitecode/generate/{id da equipe}?maxUsages=3
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// APenas o manager do projeto que essa equipe pertence ou o lider da equipe podem gerar codigos de convite
        /// </remarks>
        /// <param name="equipId">equipe para qual o dev sera adicionado ao usar o convite</param>
        /// <param name="maxUsages">numero de usos que esse convite tem</param>
        /// <returns>uma string que é o codigo a ser usado para entrar na equipe</returns>
        [HttpPost("generate/{equipId}")]
        [Authorize(Roles = "MNG,DEV")]
        public async Task<ActionResult<string>> GenerateInviteCode(string equipId, [FromQuery] int maxUsages)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null)
            {
                return Unauthorized("Please sign in to create invitation codes.");
            }

            if (User.IsInRole("MNG"))
            {
                return await _inviteCodeService.GenerateInviteCode(userIdFromToken, "MNG", equipId, maxUsages);
            }
            if (User.IsInRole("DEV"))
            {
                return await _inviteCodeService.GenerateInviteCode(userIdFromToken, "DEV", equipId, maxUsages);
            }
            else
            {
                return "You must be logged in to access this feature.";
            }
        }

        /// <summary>
        /// Entrar uma equipe utilizando um codigo de convite
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// Usado por devs comuns, que não estejam na equipe
        /// </remarks>
        /// <param name="code">codigo de convite</param>
        /// <returns>sem retorno</returns>
        /// <response code="204">Usuario logado adicionado com sucesso à equipe</response>
        [HttpPut("enter/{code}")]
        [Authorize(Roles = "DEV")]
        public async Task<IActionResult> UseInviteCode(string code)
        {
            var devIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (devIdFromToken == null)
            {
                return Unauthorized("You must be logged in to join a team.");
            }

            var addedToEquip = await _inviteCodeService.UseInviteCode(devIdFromToken, code);

            if (!addedToEquip)
            {
                return BadRequest("You cannot be added to this team.");
            }
            
            return NoContent();
        }
    }
}