using CodeNetEmployeeProjectTask.Models;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task<Employee> AddEmployeeAsync(Employee employee);
    Task<Employee> UpdateEmployeeAsync(Employee employee);
    Task<IEnumerable<Employee>> GetEmployeesByIdsAsync(IEnumerable<int> employeeIds);
    Task SaveChangesAsync();

}
