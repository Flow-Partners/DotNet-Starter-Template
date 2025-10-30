using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.Users;
using DotNet_Starter_Template.Models.ViewModels.Users;

namespace DotNet_Starter_Template.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<PagedResult<UserListViewModel>>> GetAllUsersAsync(PaginationRequest request);
        Task<ApiResponse<UserDetailViewModel?>> GetUserByIdAsync(string id);
        Task<ApiResponse<UserDetailViewModel>> CreateUserAsync(CreateUserDto createUserDto);
        Task<ApiResponse<UserDetailViewModel?>> UpdateUserAsync(string id, UpdateUserDto updateUserDto);
        Task<ApiResponse<bool>> DeleteUserAsync(string id);
        Task<ApiResponse<bool>> ActivateUserAsync(string id, bool isActive);
        Task<ApiResponse<bool>> ResetUserPasswordAsync(string id, string newPassword);
        Task<ApiResponse<List<UserRoleViewModel>>> GetUserRolesAsync(string userId);
        Task<ApiResponse<bool>> AssignRoleToUserAsync(string userId, string roleId, string assignedBy);
        Task<ApiResponse<bool>> RemoveRoleFromUserAsync(string userId, string roleId);
        Task<ApiResponse<List<string>>> GetUserPermissionsAsync(string userId);
        Task<ApiResponse<bool>> IsEmailUniqueAsync(string email, string? excludeUserId = null);
        Task<ApiResponse<bool>> IsUserNameUniqueAsync(string userName, string? excludeUserId = null);
    }
}

