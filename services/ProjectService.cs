using DTOs;
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

        public async Task<List<Project>> GetAllProjectsByManagerId(string managerId)
        {
            return await _projectRepository.GetAllMyProjects(managerId);
        }

        public async Task<Project> CreateProjectAsync(string managerId, CreateProjectDTO project)
        {
            Project newProject = new Project
            {
                Name = project.Name,
                Description = project.Description,
                Goals = project.Goals,
                ManagerId = managerId
            };

            newProject.Id = $"PROJ-{DateTime.Now.ToString("yyyyMMddHHmmssff")}";

            return await _projectRepository.CreateProjectAsync(newProject);
        }

        public async Task<Project> UpdateProjectAsync(string projectId, UpdateProjectDTO project){
            var foundedProject = await _projectRepository.GetProjectByIdAsync(projectId);

            if (project.Name != null)
            {
                foundedProject.Name = project.Name;
            }

            if (project.Goals != null)
            {
                foundedProject.Goals = project.Goals;
            }

            if (project.Description != null)
            {
                foundedProject.Description = project.Description;
            }

            return await _projectRepository.UpdateProjectAsync(foundedProject);
        }

        public async Task<bool> DeleteProjectAsync(string id){
            return await _projectRepository.DeleteProjectAsync(id);
        }
    }
}