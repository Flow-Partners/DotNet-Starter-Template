using System.ComponentModel.DataAnnotations;

namespace DotNet_Starter_Template.Models.DTOs.PrintingTemplates
{
    public class UpdatePrintingTemplateDto
    {
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
    }
}
