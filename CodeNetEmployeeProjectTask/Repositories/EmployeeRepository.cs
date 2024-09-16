using Microsoft.EntityFrameworkCore;
using CodeNetEmployeeProjectTask.Data;
using CodeNetEmployeeProjectTask.Models;
using CodeNetEmployeeProjectTask.Exceptions;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await _context.Employees
            .Include(e => e.EmployeeProjects)
            .ThenInclude(ep => ep.Project)
            .ToListAsync();
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.EmployeeProjects)
            .ThenInclude(ep => ep.Project)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);
    }
    public async Task<Employee> UpdateEmployeeAsync(Employee employee)
    {
        var existingEmployee = await _context.Employees.FindAsync(employee.EmployeeId);

        if (existingEmployee == null)
        {
            throw new NotFoundException("Employee not found.");
        }

        existingEmployee.Name = employee.Name;
        existingEmployee.Email = employee.Email;

        await _context.SaveChangesAsync();
        return existingEmployee;
    }


    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByIdsAsync(IEnumerable<int> employeeIds)
    {
        return await _context.Employees
            .Where(e => employeeIds.Contains(e.EmployeeId))
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
