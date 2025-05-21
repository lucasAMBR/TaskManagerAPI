using System.Security.Claims;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/task")]
    public class TaskController : ControllerBase
    {

        private readonly ITaskService _taskService;
        private readonly IEquipService _equipService;

        public TaskController(ITaskService taskService, IEquipService equipService)
        {
            _taskService = taskService;
            _equipService = equipService;
        }

        [HttpGet("{equipId}")]
        [Authorize(Roles = "MNG, DEV")]
        public async Task<ActionResult<List<Models.Task>>> GetTasksByEquipId(string equipId)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var equip = await _equipService.GetEquipByIdAsync(equipId);

            bool isMember = equip.Members.Any(m => m.Id == userIdFromToken);

            if (equip.Project == null)
            {
                return BadRequest("");
            }

            if (User.IsInRole("DEV"))
            {
                if (!isMember)
                {
                    return Unauthorized("You cannot see a task in a someone equip!");
                }
            }
            if (User.IsInRole("MNG"))
            {
                if (equip.Project.ManagerId != userIdFromToken)
                {
                    return Unauthorized("You cannot see a task in a someone equip!");
                }
            }

            return await _taskService.GetTasksByEquipId(equipId);
        }
    }
}