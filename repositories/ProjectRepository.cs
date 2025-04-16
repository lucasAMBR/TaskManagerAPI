using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories{
    public class ProjectRepository : IProjectRepository{

        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context){
            _context = context;
        }

        public async Task<List<Project>> GetAllProjectsAsync(){
            return await _context.Projects.ToListAsync();
        }

        
        public async Task<Project> GetProjectByIdAsync(string id){
            return await _context.Projects.FindAsync(id) ?? throw new Exception($"Project {id} not found!!");
        }

        public async Task<Project> CreateProjectAsync(Project project){
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> UpdateProjectAsync(Project project){
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<bool> DeleteProjectAsync(string id){
            var project = await _context.Projects.FindAsync(id);
            if(project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}