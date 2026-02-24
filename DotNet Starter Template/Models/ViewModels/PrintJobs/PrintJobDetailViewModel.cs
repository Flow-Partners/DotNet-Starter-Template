namespace DotNet_Starter_Template.Models.ViewModels.PrintJobs
{
    public class PrintJobDetailViewModel
    {
        public int Id { get; set; }
        public int PrinterId { get; set; }
        public string PrinterName { get; set; } = string.Empty;
        public int PrintingTemplateId { get; set; }
        public string PrintingTemplateName { get; set; } = string.Empty;
        public string? RequestedById { get; set; }
        public string? RequestedByUserName { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? JobData { get; set; }
        public int Copies { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
