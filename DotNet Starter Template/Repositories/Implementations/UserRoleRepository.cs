using Microsoft.EntityFrameworkCore;
using DotNet_Starter_Template.Data;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Repositories.Interfaces;

namespace DotNet_Starter_Template.Repositories.Implementations
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserRole>> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserRole>> GetByRoleIdAsync(string roleId)
        {
            return await _dbSet
                .Where(ur => ur.RoleId == roleId)
                .Include(ur => ur.User)
                .ToListAsync();
        }

        public async Task<UserRole?> GetByUserAndRoleAsync(string userId, string roleId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }

        public async Task<IEnumerable<UserRole>> GetActiveAssignmentsAsync()
        {
            return await _dbSet
                .Where(ur => ur.IsActive)
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleId)
        {
            return await _dbSet
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId && ur.IsActive);
        }

        public async Task<int> GetUserRoleCountAsync(string userId)
        {
            return await _dbSet
                .CountAsync(ur => ur.UserId == userId && ur.IsActive);
        }

        public async Task<int> GetRoleUserCountAsync(string roleId)
        {
            return await _dbSet
                .CountAsync(ur => ur.RoleId == roleId && ur.IsActive);
        }
    }
}

