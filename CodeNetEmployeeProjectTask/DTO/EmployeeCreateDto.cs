using System.ComponentModel.DataAnnotations;

namespace CodeNetEmployeeProjectTask.DTO
{
    public class EmployeeCreateDto
    {
        public int? EmployeeId { get; set; }

     
        public required string Name { get; set; }

     
        public string? Email { get; set; }
    }
}
