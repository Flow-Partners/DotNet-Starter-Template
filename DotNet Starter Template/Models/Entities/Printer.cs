using System.ComponentModel.DataAnnotations;

namespace DotNet_Starter_Template.Models.Entities
{
    public class Printer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? ConnectionInfo { get; set; }

        [StringLength(50)]
        public string PrinterType { get; set; } = "Network";

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<PrintJob> PrintJobs { get; set; } = new List<PrintJob>();
    }
}
