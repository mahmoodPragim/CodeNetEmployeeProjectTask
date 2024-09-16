namespace CodeNetEmployeeProjectTask.DTO
{
    public class ApiResponse<T>
    {
        public required string Status { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
        public Dictionary<string, IEnumerable<string>>? Details { get; set; } 
    }
}

