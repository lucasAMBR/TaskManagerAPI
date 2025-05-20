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

        [HttpPost("generate/{equipId}")]
        [Authorize(Roles = "MNG,DEV")]
        public async Task<ActionResult<string>> GenerateInviteCode(string equipId, [FromQuery] int maxUsages)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null)
            {
                return Unauthorized("You must be logged to generate a invite code");
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
                return "You must be logged in";
            }
        }

        [HttpPut("enter/{code}")]
        [Authorize(Roles = "DEV")]
        public async Task<IActionResult> UseInviteCode(string code)
        {
            var devIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (devIdFromToken == null)
            {
                return Unauthorized("You must be logged in to entre in a equip");
            }

            var addedToEquip = await _inviteCodeService.UseInviteCode(devIdFromToken, code);

            if (!addedToEquip)
            {
                return BadRequest("You cannot be added to this equip");
            }
            
            return NoContent();
        }
    }
}