namespace Interfaces
{
    public interface ITaskRepository
    {
        Task<Models.Task> GetByIdAsync(string taskId);
        Task<List<Models.Task>> GetTasksByEquipId(string equipId);
        Task<List<Models.Task>> GetTasksByUserId(string devId);
        Task<Models.Task> CreateTaskAsync(Models.Task task);
        Task<bool> AssignTaskAsync(string taskId, string devId);
        Task<bool> ConcludeTaskAsync(string taskId);
    }
}