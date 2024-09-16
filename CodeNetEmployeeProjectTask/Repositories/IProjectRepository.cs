using CodeNetEmployeeProjectTask.Models;
using CodeNetEmployeeProjectTask.Repositories;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task<Project> GetProjectByIdAsync(int id);
    Task<Project> AddProjectAsync(Project project);
    Task<bool> SaveChangesAsync();
    Task<IEnumerable<Project>> GetProjectsByIdsAsync(IEnumerable<int> projectIds);
    Task<Project> UpdateProjectAsync(Project project);
   

}