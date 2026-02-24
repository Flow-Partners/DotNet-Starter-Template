using System.ComponentModel.DataAnnotations;

namespace DotNet_Starter_Template.Models.DTOs.PrintJobs
{
    public class CreatePrintJobDto
    {
        public int PrinterId { get; set; }
        public int PrintingTemplateId { get; set; }
        public string? JobData { get; set; }
        public int Copies { get; set; } = 1;
    }
}
