using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories
{
    public class TaskRepository : ITaskRepository
    {

        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Task> GetByIdAsync(string taskId)
        {
            return await _context.Tasks.FindAsync(taskId) ?? throw new Exception("Task not found!");
        }

        public async Task<List<Models.Task>> GetTasksByEquipId(string equipId)
        {
            return await _context.Tasks.Where(t => t.EquipId == equipId).ToListAsync();
        }

        public async Task<List<Models.Task>> GetTasksByUserId(string userId)
        {
            return await _context.Tasks.Where(t => t.AssigneeId == userId).ToListAsync();
        }

        public async Task<Models.Task> CreateTaskAsync(Models.Task task)
        {
            task.Equip = await _context.Equips.FindAsync(task.EquipId) ?? throw new Exception("Equip not found!!");

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> AssignTaskAsync(string taskId, string devId)
        {
            Models.Task task = await GetByIdAsync(taskId);
            var assignee = await _context.Devs.FindAsync(devId) ?? throw new Exception("Dev not found!");

            task.AssigneeId = assignee.Id;

            task.AssigneeId = devId;
            task.Assignee = assignee;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConcludeTaskAsync(string taskId)
        {
            Models.Task task = await GetByIdAsync(taskId);

            task.IsDone = true;
            return true;
        }
    }
}