using Microsoft.AspNetCore.Identity;

namespace DotNet_Starter_Template.Models.Entities
{
    public class Role : IdentityRole
    {
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSystemRole { get; set; } = false;
        public int Priority { get; set; } = 0;
        public string? ParentRoleId { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public string? Category { get; set; }

        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}

