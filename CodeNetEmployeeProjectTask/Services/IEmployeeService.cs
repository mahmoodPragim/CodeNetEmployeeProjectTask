using CodeNetEmployeeProjectTask.Models;
using CodeNetEmployeeProjectTask.DTO;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task<Employee> AddEmployeeAsync(Employee employee);
    Task<Employee> UpdateEmployeeAsync(Employee employee);
    Task<IEnumerable<Employee>> GetEmployeesByIdsAsync(IEnumerable<int> employeeIds);
    Task<bool> AssignEmployeesToProjectsAsync(AssignEmployeesOnProjectsRequest request);
}
