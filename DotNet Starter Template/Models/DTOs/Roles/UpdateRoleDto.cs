using System.ComponentModel.DataAnnotations;

namespace DotNet_Starter_Template.Models.DTOs.Roles
{
    public class UpdateRoleDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        public int Priority { get; set; } = 0;
        public string? ParentRoleId { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public string? Category { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

