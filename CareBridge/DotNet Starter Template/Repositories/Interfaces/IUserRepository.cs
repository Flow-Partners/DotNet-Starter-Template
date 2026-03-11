using CareBridge.Models.Entities;
using System.Linq.Expressions;

namespace CareBridge.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUserNameAsync(string userName);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string roleId);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
        Task<bool> IsEmailUniqueAsync(string email, string? excludeUserId = null);
        Task<bool> IsUserNameUniqueAsync(string userName, string? excludeUserId = null);
    }
}

