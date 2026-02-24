using System.ComponentModel.DataAnnotations;

namespace DotNet_Starter_Template.Models.Entities
{
    public class PrintJob
    {
        public int Id { get; set; }

        public int PrinterId { get; set; }
        public int PrintingTemplateId { get; set; }

        [StringLength(450)]
        public string? RequestedById { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        public string? JobData { get; set; }
        public int Copies { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        [StringLength(1000)]
        public string? ErrorMessage { get; set; }

        public virtual Printer Printer { get; set; } = null!;
        public virtual PrintingTemplate PrintingTemplate { get; set; } = null!;
        public virtual User? RequestedBy { get; set; }
    }
}
