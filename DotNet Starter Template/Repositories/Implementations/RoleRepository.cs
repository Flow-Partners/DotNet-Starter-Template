using Microsoft.EntityFrameworkCore;
using DotNet_Starter_Template.Data;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Repositories.Interfaces;

namespace DotNet_Starter_Template.Repositories.Implementations
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<IEnumerable<Role>> GetActiveRolesAsync()
        {
            return await _dbSet
                .Where(r => r.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetRolesByCategoryAsync(string category)
        {
            return await _dbSet
                .Where(r => r.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetSystemRolesAsync()
        {
            return await _dbSet
                .Where(r => r.IsSystemRole)
                .ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, string? excludeRoleId = null)
        {
            var query = _dbSet.Where(r => r.Name == name);
            if (!string.IsNullOrEmpty(excludeRoleId))
            {
                query = query.Where(r => r.Id != excludeRoleId);
            }
            return !await query.AnyAsync();
        }
    }
}

