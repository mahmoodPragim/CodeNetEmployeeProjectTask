using System.ComponentModel.DataAnnotations;

namespace CodeNetEmployeeProjectTask.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<EmployeeProject>? EmployeeProjects { get; set; }
    }
}
