namespace DotNet_Starter_Template.Models.Common
{
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? CorrelationId { get; set; }
        public string? StackTrace { get; set; }
    }
}

