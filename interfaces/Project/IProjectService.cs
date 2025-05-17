using DTOs;
using Models;

namespace Interfaces{
    public interface IProjectService{
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(string id);
        Task<Project> CreateProjectAsync(string id, CreateProjectDTO project);
        Task<Project> UpdateProjectAsync(string projectId, UpdateProjectDTO project);
        Task<bool> DeleteProjectAsync(string id);
    }
}