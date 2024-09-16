using Microsoft.EntityFrameworkCore;
using CodeNetEmployeeProjectTask.Data;
using CodeNetEmployeeProjectTask.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeNetEmployeeProjectTask.Repositories
{


    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public async Task<Project> AddProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }
        public async Task<Project> UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);  
            await _context.SaveChangesAsync();  
            return project;  
        }



        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Project>> GetProjectsByIdsAsync(IEnumerable<int> projectIds)
        {
            return await _context.Projects
                .Where(p => projectIds.Contains(p.ProjectId))
                .ToListAsync();
        }
    }
}
