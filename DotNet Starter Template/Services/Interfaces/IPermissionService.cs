using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.Permissions;
using DotNet_Starter_Template.Models.ViewModels.Permissions;

namespace DotNet_Starter_Template.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<ApiResponse<PagedResult<PermissionListViewModel>>> GetAllPermissionsAsync(PaginationRequest request);
        Task<ApiResponse<PermissionDetailViewModel?>> GetPermissionByIdAsync(int id);
        Task<ApiResponse<PermissionDetailViewModel>> CreatePermissionAsync(CreatePermissionDto createPermissionDto);
        Task<ApiResponse<PermissionDetailViewModel?>> UpdatePermissionAsync(int id, UpdatePermissionDto updatePermissionDto);
        Task<ApiResponse<bool>> DeletePermissionAsync(int id);
        Task<ApiResponse<bool>> ActivatePermissionAsync(int id, bool isActive);
        Task<ApiResponse<List<PermissionRoleViewModel>>> GetPermissionRolesAsync(int permissionId);
        Task<ApiResponse<List<string>>> GetModulesAsync();
        Task<ApiResponse<List<string>>> GetActionsAsync();
        Task<ApiResponse<List<string>>> GetCategoriesAsync();
        Task<ApiResponse<bool>> IsNameUniqueAsync(string name, int? excludePermissionId = null);
    }
}

