namespace ToolsTrackPro.API.Models
{
    public class ApiResponse<T>
    {
        public string Status { get; set; } // "success", "fail", "error"
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public ApiResponse(string status)
        {
            Status = status;
        }
        public ApiResponse(string status, T data)
        {
            Status = status;
            Data = data;
        }

        public ApiResponse(string status, List<string> errors)
        {
            Status = status;
            Errors = errors;
        }
    }
}
