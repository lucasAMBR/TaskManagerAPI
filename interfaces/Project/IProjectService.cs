using Models;

namespace Interfaces{
    public interface IProjectService{
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(string id);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(string id);
    }
}