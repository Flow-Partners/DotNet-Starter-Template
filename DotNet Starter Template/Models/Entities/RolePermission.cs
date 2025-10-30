namespace DotNet_Starter_Template.Models.Entities
{
    public class RolePermission
    {
        public string RoleId { get; set; } = string.Empty;
        public int PermissionId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public string AssignedBy { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Conditions { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public virtual Role Role { get; set; } = null!;
        public virtual Permission Permission { get; set; } = null!;
    }
}

