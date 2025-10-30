namespace DotNet_Starter_Template.Models.ViewModels.Permissions
{
    public class PermissionListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Group { get; set; }
        public int Level { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public int Priority { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystemPermission { get; set; }
        public bool IsDeprecated { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RoleCount { get; set; }
    }
}

