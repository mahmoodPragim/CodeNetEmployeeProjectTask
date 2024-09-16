using System.ComponentModel.DataAnnotations;

namespace CodeNetEmployeeProjectTask.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Email { get; set; }

        public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
    }
}
