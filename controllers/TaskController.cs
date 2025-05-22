using System.Security.Claims;
using DTOs;
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
        private readonly IConclusionNoteService _conclusionNoteService;

        public TaskController(ITaskService taskService, IEquipService equipService, IConclusionNoteService conclusionNoteService)
        {
            _taskService = taskService;
            _equipService = equipService;
            _conclusionNoteService = conclusionNoteService;
        }

        [HttpGet("equip/{equipId}")]
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

        [HttpGet("my-tasks")]
        [Authorize(Roles = "DEV")]
        public async Task<ActionResult<List<Models.Task>>> GetTasksByUserId()
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null)
            {
                return Unauthorized("You must be logged to see your tasks");
            }

            return await _taskService.GetTasksByUserId(userIdFromToken);
        }

        [HttpPost("{equipId}/create")]
        [Authorize(Roles = "DEV, MNG")]
        public async Task<ActionResult<Models.Task>> CreateTask(string equipId, CreateTaskDTO task)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null)
            {
                return Unauthorized("You must be logged in to create a task");
            }

            Equip equip = await _equipService.GetEquipByIdAsync(equipId);

            if (User.IsInRole("DEV"))
            {
                if (userIdFromToken != equip.LeaderId)
                {
                    return Unauthorized("You aren't the leader of this equip");
                }
            }

            if (User.IsInRole("MNG"))
            {
                if (equip.Project == null)
                {
                    return BadRequest("Project doent exist");
                }
                if (userIdFromToken != equip.Project.ManagerId)
                {
                    return Unauthorized("You aren't the leader of this equip");
                }
            }

            return await _taskService.CreateTaskAsync(equipId, task);
        }

        [HttpPut("{taskId}/assign/{devId}")]
        [Authorize(Roles = "DEV, MNG")]
        public async Task<ActionResult<bool>> AssignTask(string taskId, string devId)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null)
            {
                return Unauthorized("You must be logged in to create a task");
            }

            Models.Task task = await _taskService.GetByIdAsync(taskId);
            Equip equip = await _equipService.GetEquipByIdAsync(task.EquipId);

            bool isLeader = userIdFromToken == equip.LeaderId;
            bool isManager = equip.Project != null && userIdFromToken == equip.Project.ManagerId;
            bool isDevSelfAssigning = userIdFromToken == devId && equip.Members.Any(m => m.Id == devId);

            if (User.IsInRole("DEV"))
            {
                if (isDevSelfAssigning)
                {
                    if (isLeader)
                    {
                        bool leaderAutoAssign = await _taskService.AssignTaskAsync(taskId, devId);

                        return NoContent();
                    }
                    if (task.AssigneeId != null)
                    {
                        return BadRequest("This task is already assigned");
                    }

                    bool autoAssigned = await _taskService.AssignTaskAsync(taskId, devId);

                    if (!autoAssigned)
                    {
                        return BadRequest("Somthing go wrong during assignment");
                    }

                    return NoContent();
                }

                if (!isLeader)
                {
                    return Unauthorized("You aren't the leader of this equip");
                }
            }

            if (User.IsInRole("MNG"))
            {
                if (equip.Project == null)
                {
                    return BadRequest("Project doent exist");
                }
                if (!isManager)
                {
                    return Unauthorized("You aren't the manager of this project");
                }
            }

            bool assigned = await _taskService.AssignTaskAsync(taskId, devId);

            if (!assigned)
            {
                return BadRequest("Somthing go wrong during assignment");
            }

            return NoContent();
        }

        [HttpPut("conclude/{taskId}")]
        [Authorize(Roles = "DEV")]
        public async Task<ActionResult<ConclusionNote>> ConcludeTask(string taskId, CreateConclusionNoteDTO note)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null)
            {
                return Unauthorized("You must be logged in to conclude a task");
            }

            Models.Task task = await _taskService.GetByIdAsync(taskId);

            if (userIdFromToken != task.AssigneeId)
            {
                return BadRequest("You cannot end someone task");
            }

            var isConcluded = await _taskService.ConcludeTaskAsync(taskId);

            if (!isConcluded)
            {
                return BadRequest("Something go wrong in conclusion");
            }

            var conclusionNote = await _conclusionNoteService.GenerateConclusionNote(taskId, note);

            return conclusionNote;
        }
    }
}