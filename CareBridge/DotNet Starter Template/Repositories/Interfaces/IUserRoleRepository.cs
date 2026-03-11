using CareBridge.Models.Entities;

namespace CareBridge.Repositories.Interfaces
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<IEnumerable<UserRole>> GetByUserIdAsync(string userId);
        Task<IEnumerable<UserRole>> GetByRoleIdAsync(string roleId);
        Task<UserRole?> GetByUserAndRoleAsync(string userId, string roleId);
        Task<IEnumerable<UserRole>> GetActiveAssignmentsAsync();
        Task<bool> IsUserInRoleAsync(string userId, string roleId);
        Task<int> GetUserRoleCountAsync(string userId);
        Task<int> GetRoleUserCountAsync(string roleId);
    }
}

