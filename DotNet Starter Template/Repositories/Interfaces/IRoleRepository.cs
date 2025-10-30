using DotNet_Starter_Template.Models.Entities;

namespace DotNet_Starter_Template.Repositories.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetActiveRolesAsync();
        Task<IEnumerable<Role>> GetRolesByCategoryAsync(string category);
        Task<IEnumerable<Role>> GetSystemRolesAsync();
        Task<bool> IsNameUniqueAsync(string name, string? excludeRoleId = null);
    }
}

