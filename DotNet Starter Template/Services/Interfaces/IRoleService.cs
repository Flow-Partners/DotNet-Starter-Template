using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.Roles;
using DotNet_Starter_Template.Models.ViewModels.Roles;

namespace DotNet_Starter_Template.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<PagedResult<RoleListViewModel>>> GetAllRolesAsync(PaginationRequest request);
        Task<ApiResponse<RoleDetailViewModel?>> GetRoleByIdAsync(string id);
        Task<ApiResponse<RoleDetailViewModel>> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<ApiResponse<RoleDetailViewModel?>> UpdateRoleAsync(string id, UpdateRoleDto updateRoleDto);
        Task<ApiResponse<bool>> DeleteRoleAsync(string id);
        Task<ApiResponse<bool>> ActivateRoleAsync(string id, bool isActive);
        Task<ApiResponse<List<RolePermissionViewModel>>> GetRolePermissionsAsync(string roleId);
        Task<ApiResponse<bool>> AssignPermissionToRoleAsync(string roleId, int permissionId, string assignedBy);
        Task<ApiResponse<bool>> RemovePermissionFromRoleAsync(string roleId, int permissionId);
        Task<ApiResponse<List<RoleUserViewModel>>> GetRoleUsersAsync(string roleId);
        Task<ApiResponse<bool>> IsNameUniqueAsync(string name, string? excludeRoleId = null);
    }
}

