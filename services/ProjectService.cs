using Interfaces;
using Models;

namespace Services{
    public class ProjectService : IProjectService {

        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository){
            _projectRepository = projectRepository;
        } 

        public async Task<List<Project>> GetAllProjectsAsync(){
            return await _projectRepository.GetAllProjectsAsync();
        }

        public async Task<Project> GetProjectByIdAsync(string id){
            return await _projectRepository.GetProjectByIdAsync(id);
        }

        public async Task<Project> CreateProjectAsync(Project project){
            project.Id = $"PROJ-{DateTime.Now.ToString("yyyyMMddHHmmssff")}";
            
            return await _projectRepository.CreateProjectAsync(project);
        }

        public async Task<Project> UpdateProjectAsync(Project project){
            return await _projectRepository.UpdateProjectAsync(project);
        }

        public async Task<bool> DeleteProjectAsync(string id){
            return await _projectRepository.DeleteProjectAsync(id);
        }
    }
}