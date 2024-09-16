using CodeNetEmployeeProjectTask.Models;

namespace CodeNetEmployeeProjectTask.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _projectRepository.GetAllProjectsAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _projectRepository.GetProjectByIdAsync(id);
        }
        public async Task<Project> UpdateProjectAsync(Project project)
        {
          return  await  _projectRepository.UpdateProjectAsync(project);
        }


        public async Task<Project> AddProjectAsync(Project project)
        {
            return await _projectRepository.AddProjectAsync(project);
        }

        public async Task<IEnumerable<Project>> GetProjectsByIdsAsync(IEnumerable<int> projectIds)
        {
            return await _projectRepository.GetProjectsByIdsAsync(projectIds);
        }
    }
}
