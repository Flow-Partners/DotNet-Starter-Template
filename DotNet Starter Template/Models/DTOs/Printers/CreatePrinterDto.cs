using System.ComponentModel.DataAnnotations;

namespace DotNet_Starter_Template.Models.DTOs.Printers
{
    public class CreatePrinterDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? ConnectionInfo { get; set; }

        [StringLength(50)]
        public string PrinterType { get; set; } = "Network";
    }
}
