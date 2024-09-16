using CodeNetEmployeeProjectTask.Models;
using CodeNetEmployeeProjectTask.DTO;

namespace CodeNetEmployeeProjectTask.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProjectRepository _projectRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IProjectRepository projectRepository)
        {
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            return await _employeeRepository.AddEmployeeAsync(employee);
        }
        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            return await _employeeRepository.UpdateEmployeeAsync(employee);
        }


        public async Task<IEnumerable<Employee>> GetEmployeesByIdsAsync(IEnumerable<int> employeeIds)
        {
            return await _employeeRepository.GetEmployeesByIdsAsync(employeeIds);
        }

        public async Task<bool> AssignEmployeesToProjectsAsync(AssignEmployeesOnProjectsRequest request)
        {
            var employees = await _employeeRepository.GetEmployeesByIdsAsync(request.EmployeeIds);
            var projects = await _projectRepository.GetProjectsByIdsAsync(request.ProjectIds);

            if (employees.Count() != request.EmployeeIds.Count() || projects.Count() != request.ProjectIds.Count())
            {
                return false;
            }
            foreach (var employee in employees)
            {
                foreach (var project in projects)
                {
                    if (employee.EmployeeProjects == null)
                    {
                        employee.EmployeeProjects = new List<EmployeeProject>();
                    }

                    if (!employee.EmployeeProjects.Any(ep => ep.ProjectId == project.ProjectId))
                    {
                        employee.EmployeeProjects.Add(new EmployeeProject
                        {
                            EmployeeId = employee.EmployeeId,
                            ProjectId = project.ProjectId,
                            Employee = employee,
                            Project = project
                        });
                    }
                }
            }

            await _employeeRepository.SaveChangesAsync();

            return true;
        }
    }
}
