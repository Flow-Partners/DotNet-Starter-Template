using System.ComponentModel.DataAnnotations;

namespace DotNet_Starter_Template.Models.Entities
{
    public class PrintingTemplate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [StringLength(50)]
        public string? TemplateType { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<PrintJob> PrintJobs { get; set; } = new List<PrintJob>();
    }
}
