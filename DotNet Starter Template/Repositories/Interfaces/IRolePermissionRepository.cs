using DotNet_Starter_Template.Models.Entities;

namespace DotNet_Starter_Template.Repositories.Interfaces
{
    public interface IRolePermissionRepository : IRepository<RolePermission>
    {
        Task<IEnumerable<RolePermission>> GetByRoleIdAsync(string roleId);
        Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(int permissionId);
        Task<RolePermission?> GetByRoleAndPermissionAsync(string roleId, int permissionId);
        Task<IEnumerable<RolePermission>> GetActiveAssignmentsAsync();
        Task<bool> IsRoleHasPermissionAsync(string roleId, int permissionId);
        Task<int> GetRolePermissionCountAsync(string roleId);
        Task<int> GetPermissionRoleCountAsync(int permissionId);
    }
}

