using System.ComponentModel.DataAnnotations;

namespace CodeNetEmployeeProjectTask.DTO
{
    public class ProjectCreateDto
    {
        public int? ProjectId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
