using Models;

namespace Interfaces{
    public interface IProjectRepository{
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(string id);
        Task<List<Project>> GetAllMyProjects(string managerId);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(string id);
    }
}