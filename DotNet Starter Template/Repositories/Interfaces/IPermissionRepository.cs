using DotNet_Starter_Template.Models.Entities;

namespace DotNet_Starter_Template.Repositories.Interfaces
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<Permission?> GetByNameAsync(string name);
        Task<IEnumerable<Permission>> GetByModuleAsync(string module);
        Task<IEnumerable<Permission>> GetByActionAsync(string action);
        Task<IEnumerable<Permission>> GetByCategoryAsync(string category);
        Task<IEnumerable<Permission>> GetActivePermissionsAsync();
        Task<IEnumerable<Permission>> GetSystemPermissionsAsync();
        Task<IEnumerable<string>> GetModulesAsync();
        Task<IEnumerable<string>> GetActionsAsync();
        Task<bool> IsNameUniqueAsync(string name, int? excludePermissionId = null);
    }
}

