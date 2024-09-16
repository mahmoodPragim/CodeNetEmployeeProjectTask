using CodeNetEmployeeProjectTask.Models;

namespace CodeNetEmployeeProjectTask.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task<Project> AddProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(Project project);
        Task<IEnumerable<Project>> GetProjectsByIdsAsync(IEnumerable<int> projectIds);

    }
}
