using DTOs;
using Interfaces;

namespace Services
{
    public class TaskService : ITaskService
    {

        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Models.Task> GetByIdAsync(string taskId)
        {
            return await _taskRepository.GetByIdAsync(taskId);
        }

        public async Task<List<Models.Task>> GetTasksByEquipId(string equipId)
        {
            return await _taskRepository.GetTasksByEquipId(equipId);
        }

        public async Task<List<Models.Task>> GetTasksByUserId(string userId)
        {
            return await _taskRepository.GetTasksByUserId(userId);
        }

        public async Task<Models.Task> CreateTaskAsync(CreateTaskDTO task)
        {
            Models.Task newTask = new Models.Task
            {
                Description = task.Description,
                Priority = task.Priority,
                InitialDate = task.InitialDate,
                FinalDate = task.FinalDate,
                EquipId = task.EquipId
            };

            if (task.AssigneeId != null)
            {
                newTask.AssigneeId = task.AssigneeId;
            }

            return await _taskRepository.CreateTaskAsync(newTask);
        }

        public async Task<bool> AssignTaskAsync(string taskId, string devId)
        {
            var assigned = await _taskRepository.AssignTaskAsync(taskId, devId);

            return assigned;
        }

        public async Task<bool> ConcludeTaskAsync(string taskId)
        {
            var concluded = await _taskRepository.ConcludeTaskAsync(taskId);

            return concluded;
        }
    }
}
