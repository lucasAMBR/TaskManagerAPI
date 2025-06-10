using System.Security.Claims;
using DTOs;
using Formatter;
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

        /// <summary>
        /// Lista todas as tasks de um equipe
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// Apenas o manager e membros da equipe podem ver essas tasks
        /// </remarks>
        /// <param name="equipId">id da equipe</param>
        /// <returns>lista de tasks, cada um com os seguintes campos: id, description, priority, initialDate, finalDate, equipId, equipDepartament, assigneeId, assigneeName, isDone</returns>
        [HttpGet("equip/{equipId}")]
        [Authorize(Roles = "MNG, DEV")]
        public async Task<ActionResult<List<FormattedTaskDTO>>> GetTasksByEquipId(string equipId)
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
                    return Unauthorized("You cannot view tasks in another user's team.");
                }
            }
            if (User.IsInRole("MNG"))
            {
                if (equip.Project.ManagerId != userIdFromToken)
                {
                    return Unauthorized("You cannot view tasks in another user's team.");
                }
            }

            var taskList = await _taskService.GetTasksByEquipId(equipId);

            var FormatedTaskList = taskList.Select(task => FormatterHelper.TaskFormatter(task)).ToList();

            return FormatedTaskList;
        }

        /// <summary>
        /// Lista todas as tarefas do usuario logado
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// </remarks>
        /// <returns>lista de tasks, cada um com os seguintes campos: id, description, priority, initialDate, finalDate, equipId, equipDepartament, assigneeId, assigneeName, isDone</returns>
        [HttpGet("my-tasks")]
        [Authorize(Roles = "DEV")]
        public async Task<ActionResult<List<FormattedTaskDTO>>> GetTasksByUserId()
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null)
            {
                return Unauthorized("Please sign in to access your task list.");
            }

            var rawList = await _taskService.GetTasksByUserId(userIdFromToken);

            var formatedList = rawList.Select(taskItem => FormatterHelper.TaskFormatter(taskItem)).ToList();

            return formatedList;
        }

        /// <summary>
        /// Cria uma nova tarefa para uma determinada equipe
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// </remarks>
        /// <param name="equipId">nome da equipe que recebera a nova tarefa, via URL</param>
        /// <param name="task">
        /// um objeto com os detalhes da task, contendo:
        /// - description : descrição da tarefa, oq deve ser feito,
        /// - priority : nivel de prioridade da tarefa, quanto mais alto, mais urgente (um numero inteiro)
        /// - assigneeId (opcional): Id do dev responsavel por essa tarefa, pode não ser enviado para ficar disponivel para algum dev desocupado pegar ela pra si
        /// </param>
        /// <returns>Retorna os dados da tasks cadastrada: id, description, priority, initialDate, finalDate, equipId, equipDepartament, assigneeId, assigneeName, isDone</returns>
        [HttpPost("{equipId}/create")]
        [Authorize(Roles = "DEV, MNG")]
        public async Task<ActionResult<FormattedTaskDTO>> CreateTask(string equipId, CreateTaskDTO task)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null)
            {
                return Unauthorized("You must be logged in to view your tasks.");
            }

            Equip equip = await _equipService.GetEquipByIdAsync(equipId);

            if (User.IsInRole("DEV"))
            {
                if (userIdFromToken != equip.LeaderId)
                {
                    return Unauthorized("Only the team leader can perform this action.");
                }
            }

            if (User.IsInRole("MNG"))
            {
                if (equip.Project == null)
                {
                    return BadRequest("Project not found.");
                }
                if (userIdFromToken != equip.Project.ManagerId)
                {
                    return Unauthorized("Only team leaders can perform this action.");
                }
            }

            var rawTask = await _taskService.CreateTaskAsync(equipId, task);

            return FormatterHelper.TaskFormatter(rawTask);
        }

        /// <summary>
        /// Atribui uma determinada tarefa para um dev
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// OBS: Managers do projeto e lideres podem atribuir tarefas para outros devs, inclusive pegar tarefas que ja estavam atribuidas e atribuir novamente
        /// Devs normais apenas podem pegar tarefas que não tem um responsavel atribuido para si
        /// </remarks>
        /// <param name="taskId">id da task a ser atribuida</param>
        /// <param name="devId">id do dev que sera responsavel pela tarefa</param>
        /// <returns>sem retorno</returns>
        /// <response code="204">atribuido corretamente</response>
        [HttpPut("{taskId}/assign/{devId}")]
        [Authorize(Roles = "DEV, MNG")]
        public async Task<ActionResult<bool>> AssignTask(string taskId, string devId)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null)
            {
                return Unauthorized("You must be logged in to create tasks.");
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
                        return BadRequest("This task is already assigned.");
                    }

                    bool autoAssigned = await _taskService.AssignTaskAsync(taskId, devId);

                    if (!autoAssigned)
                    {
                        return BadRequest("Task assignment failed.");
                    }

                    return NoContent();
                }

                if (!isLeader)
                {
                    return Unauthorized("Only equipment leaders can perform this action.");
                }
            }

            if (User.IsInRole("MNG"))
            {
                if (equip.Project == null)
                {
                    return BadRequest("Project not found.");
                }
                if (!isManager)
                {
                    return Unauthorized("Only project managers can perform this action.");
                }
            }

            bool assigned = await _taskService.AssignTaskAsync(taskId, devId);

            if (!assigned)
            {
                return BadRequest("Task assignment failed.");
            }

            return NoContent();
        }

        /// <summary>
        /// Conclui uma task
        /// </summary>
        /// <remarks>
        /// Endpoint protegido, para acessa-lo é necessario passar nos headers da requisição:
        /// Authorization: Bearer {token do usuario gerado no login}
        /// OBS: Apenas o responsavel pela tarefa pode concluir ela
        /// </remarks>
        /// <param name="taskId">id da tarefa</param>
        /// <param name="note">
        /// objeto com os dados do relatorio de conclusão que contem: 
        /// - type : Tipo da conclusão pode ser "Sucess", "With remarks"
        /// - note : Descrição do que foi, possiveis problemas encontrados, coisas novas que podem ser feitas a partir dessa tarefa
        /// - hoursSpend: Horas gastas para completar essa tarefa para ajudar na precificação do sistema
        /// </param>
        /// <returns>Retorna o relatorio de conclusão com: id, taskId, task (Com todos os dados da task), type, notes, hoursSpend, createdAt</returns>
        [HttpPut("conclude/{taskId}")]
        [Authorize(Roles = "DEV")]
        public async Task<ActionResult<ConclusionNote>> ConcludeTask(string taskId, CreateConclusionNoteDTO note)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdFromToken == null)
            {
                return Unauthorized("Please sign in to complete your tasks.");
            }

            Models.Task task = await _taskService.GetByIdAsync(taskId);

            if (userIdFromToken != task.AssigneeId)
            {
                return BadRequest("You can only complete your own assigned tasks.");
            }

            var isConcluded = await _taskService.ConcludeTaskAsync(taskId);

            if (!isConcluded)
            {
                return BadRequest("Task conclusion failed.");
            }

            var conclusionNote = await _conclusionNoteService.GenerateConclusionNote(taskId, note);

            return conclusionNote;
        }
    }
}