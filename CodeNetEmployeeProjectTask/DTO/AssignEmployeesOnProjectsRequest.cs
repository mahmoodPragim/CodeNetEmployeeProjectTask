namespace CodeNetEmployeeProjectTask.DTO
{
    public class AssignEmployeesOnProjectsRequest
    {
        public List<int> EmployeeIds { get; set; } = new List<int>();
        public List<int> ProjectIds { get; set; } = new List<int>();
    }
}
