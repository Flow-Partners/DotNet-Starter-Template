using Microsoft.EntityFrameworkCore;
using DotNet_Starter_Template.Data;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Repositories.Interfaces;

namespace DotNet_Starter_Template.Repositories.Implementations
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Permission?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Permission>> GetByModuleAsync(string module)
        {
            return await _dbSet
                .Where(p => p.Module == module)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetByActionAsync(string action)
        {
            return await _dbSet
                .Where(p => p.Action == action)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetByCategoryAsync(string category)
        {
            return await _dbSet
                .Where(p => p.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetActivePermissionsAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetSystemPermissionsAsync()
        {
            return await _dbSet
                .Where(p => p.IsSystemPermission)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetModulesAsync()
        {
            return await _dbSet
                .Select(p => p.Module)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetActionsAsync()
        {
            return await _dbSet
                .Select(p => p.Action)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludePermissionId = null)
        {
            var query = _dbSet.Where(p => p.Name == name);
            if (excludePermissionId.HasValue)
            {
                query = query.Where(p => p.Id != excludePermissionId.Value);
            }
            return !await query.AnyAsync();
        }
    }
}

