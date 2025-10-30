namespace DotNet_Starter_Template.Models.ViewModels.Permissions
{
    public class PermissionDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Group { get; set; }
        public string? SubModule { get; set; }
        public string? Feature { get; set; }
        public string? Component { get; set; }
        public string? Page { get; set; }
        public string? Section { get; set; }
        public int Level { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public int Priority { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystemPermission { get; set; }
        public bool IsDeprecated { get; set; }
        public int? ReplacementId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RoleCount { get; set; }
        public List<PermissionRoleViewModel> Roles { get; set; } = new List<PermissionRoleViewModel>();
    }

    public class PermissionRoleViewModel
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public DateTime AssignedAt { get; set; }
        public string AssignedBy { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}

