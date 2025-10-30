using System.ComponentModel.DataAnnotations;

namespace DotNet_Starter_Template.Models.Entities
{
    public class Permission
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Module { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Action { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Category { get; set; }

        [StringLength(50)]
        public string? Group { get; set; }

        [StringLength(50)]
        public string? SubModule { get; set; }

        [StringLength(50)]
        public string? Feature { get; set; }

        [StringLength(50)]
        public string? Component { get; set; }

        [StringLength(50)]
        public string? Page { get; set; }

        [StringLength(50)]
        public string? Section { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public bool IsSystemPermission { get; set; } = false;
        public bool IsDeprecated { get; set; } = false;
        public int? ReplacementId { get; set; }
        public int Level { get; set; } = 1;
        public string Type { get; set; } = "Functional";
        public string Scope { get; set; } = "Global";
        public int Priority { get; set; } = 0;

        // Navigation properties
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}

