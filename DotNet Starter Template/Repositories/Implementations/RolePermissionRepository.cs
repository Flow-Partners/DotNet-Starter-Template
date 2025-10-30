using Microsoft.EntityFrameworkCore;
using DotNet_Starter_Template.Data;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Repositories.Interfaces;

namespace DotNet_Starter_Template.Repositories.Implementations
{
    public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RolePermission>> GetByRoleIdAsync(string roleId)
        {
            return await _dbSet
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(int permissionId)
        {
            return await _dbSet
                .Where(rp => rp.PermissionId == permissionId)
                .Include(rp => rp.Role)
                .ToListAsync();
        }

        public async Task<RolePermission?> GetByRoleAndPermissionAsync(string roleId, int permissionId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        }

        public async Task<IEnumerable<RolePermission>> GetActiveAssignmentsAsync()
        {
            return await _dbSet
                .Where(rp => rp.IsActive)
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<bool> IsRoleHasPermissionAsync(string roleId, int permissionId)
        {
            return await _dbSet
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId && rp.IsActive);
        }

        public async Task<int> GetRolePermissionCountAsync(string roleId)
        {
            return await _dbSet
                .CountAsync(rp => rp.RoleId == roleId && rp.IsActive);
        }

        public async Task<int> GetPermissionRoleCountAsync(int permissionId)
        {
            return await _dbSet
                .CountAsync(rp => rp.PermissionId == permissionId && rp.IsActive);
        }
    }
}

