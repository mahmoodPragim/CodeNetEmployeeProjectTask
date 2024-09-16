using Microsoft.AspNetCore.Mvc;
using CodeNetEmployeeProjectTask.DTO;
using CodeNetEmployeeProjectTask.Models;
using CodeNetEmployeeProjectTask.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeNetEmployeeProjectTask.Validators;

namespace CodeNetEmployeeProjectTask.Controllers
{
    [Route("api/Employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly EmployeeValidator _validator;


        public EmployeeController(IEmployeeService employeeService, EmployeeValidator validator)
        {
            _employeeService = employeeService;
            _validator = validator;

        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Employee>>>> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            var response = new ApiResponse<IEnumerable<Employee>>
            {
                Data = employees,
                Status = "Success",
                Error = null
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Employee>>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                var response = new ApiResponse<Employee>
                {
                    Data = null,
                    Status = "Error",
                    Error = "Employee not found."
                };
                return NotFound(response);
            }

            var successResponse = new ApiResponse<Employee>
            {
                Data = employee,
                Status = "Success",
                Error = null
            };
            return Ok(successResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Employee>>> PostEmployee(EmployeeCreateDto employeeDto)
        {
            Employee emp;

            if (employeeDto.EmployeeId.HasValue)
            {
                emp = await _employeeService.GetEmployeeByIdAsync(employeeDto.EmployeeId.Value);
                if (emp == null)
                {
                    return NotFound(new ApiResponse<Employee>
                    {
                        Status = "Error",
                        Error = "Employee not found."
                    });
                }

                emp.Name = employeeDto.Name;
                emp.Email = employeeDto.Email;
            }
            else 
            {
                emp = new Employee
                {
                    Name = employeeDto.Name,
                    Email = employeeDto.Email
                };
            }

            var validationResult = _validator.Validate(emp);

            if (!validationResult.IsValid)
            {
                var response = new ApiResponse<Employee>
                {
                    Status = "Error",
                    Data = null,
                    Error = "Validation failed",
                    Details = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).AsEnumerable())
                };

                return BadRequest(response);
            }

            if (employeeDto.EmployeeId.HasValue)
            {
                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(emp);

                var responseSuccess = new ApiResponse<Employee>
                {
                    Data = updatedEmployee,
                    Status = "Success",
                    Error = null
                };

                return Ok(responseSuccess);
            }
            else
            {
                var newEmployee = await _employeeService.AddEmployeeAsync(emp);

                var responseSuccess = new ApiResponse<Employee>
                {
                    Data = newEmployee,
                    Status = "Success",
                    Error = null
                };

                return CreatedAtAction(nameof(GetEmployee), new { id = newEmployee.EmployeeId }, responseSuccess);
            }
        }

        [HttpPost("assignEmployeesOnProjects")]
        public async Task<IActionResult> AssignEmployeesOnProjects([FromBody] AssignEmployeesOnProjectsRequest request)
        {
            if (request.EmployeeIds == null || request.ProjectIds == null)
            {
                var response = new ApiResponse<object>
                {
                    Data = null,
                    Status = "Error",
                    Error = "EmployeeIds and ProjectIds must be provided."
                };
                return BadRequest(response);
            }



            var result = await _employeeService.AssignEmployeesToProjectsAsync(request);

            if (!result)
            {
                var response = new ApiResponse<object>
                {
                    Data = null,
                    Status = "Error",
                    Error = "One or more employees or projects not found."
                };
                return NotFound(response);
            }

            var successResponse = new ApiResponse<object>
            {
                Data = null,
                Status = "Success",
                Error = null
            };

            return Ok(successResponse);
        }
    }
}
