using Microsoft.EntityFrameworkCore;
using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.Roles;
using DotNet_Starter_Template.Models.ViewModels.Roles;
using DotNet_Starter_Template.Repositories.Interfaces;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public RoleService(
            IRoleRepository roleRepository,
            IRolePermissionRepository rolePermissionRepository,
            IUserRoleRepository userRoleRepository)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<ApiResponse<PagedResult<RoleListViewModel>>> GetAllRolesAsync(PaginationRequest request)
        {
            try
            {
                var roles = await _roleRepository.GetAllAsync();
                var roleViewModels = roles.Select(MapToRoleListViewModel).ToList();

                // Simple pagination (in a real app, you'd do this at the database level)
                var totalCount = roleViewModels.Count;
                var pagedItems = roleViewModels
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var pagedResult = new PagedResult<RoleListViewModel>(pagedItems, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResult<RoleListViewModel>>.SuccessResult(pagedResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<RoleListViewModel>>.ErrorResult($"Failed to get roles: {ex.Message}");
            }
        }

        public async Task<ApiResponse<RoleDetailViewModel?>> GetRoleByIdAsync(string id)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    return ApiResponse<RoleDetailViewModel?>.ErrorResult("Role not found");
                }

                var roleDetail = MapToRoleDetailViewModel(role);
                return ApiResponse<RoleDetailViewModel?>.SuccessResult(roleDetail);
            }
            catch (Exception ex)
            {
                return ApiResponse<RoleDetailViewModel?>.ErrorResult($"Failed to get role: {ex.Message}");
            }
        }

        public async Task<ApiResponse<RoleDetailViewModel>> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            try
            {
                // Check if name is unique
                var isUnique = await _roleRepository.IsNameUniqueAsync(createRoleDto.Name);
                if (!isUnique)
                {
                    return ApiResponse<RoleDetailViewModel>.ErrorResult("Role name already exists");
                }

                var role = new Models.Entities.Role
                {
                    Name = createRoleDto.Name,
                    NormalizedName = createRoleDto.Name.ToUpper(),
                    Description = createRoleDto.Description,
                    Priority = createRoleDto.Priority,
                    ParentRoleId = createRoleDto.ParentRoleId,
                    Color = createRoleDto.Color,
                    Icon = createRoleDto.Icon,
                    Category = createRoleDto.Category,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsSystemRole = false
                };

                var createdRole = await _roleRepository.CreateAsync(role);
                var roleDetail = MapToRoleDetailViewModel(createdRole);
                return ApiResponse<RoleDetailViewModel>.SuccessResult(roleDetail, "Role created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<RoleDetailViewModel>.ErrorResult($"Failed to create role: {ex.Message}");
            }
        }

        public async Task<ApiResponse<RoleDetailViewModel?>> UpdateRoleAsync(string id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    return ApiResponse<RoleDetailViewModel?>.ErrorResult("Role not found");
                }

                // Check if name is unique (excluding current role)
                var isUnique = await _roleRepository.IsNameUniqueAsync(updateRoleDto.Name, id);
                if (!isUnique)
                {
                    return ApiResponse<RoleDetailViewModel?>.ErrorResult("Role name already exists");
                }

                role.Name = updateRoleDto.Name;
                role.NormalizedName = updateRoleDto.Name.ToUpper();
                role.Description = updateRoleDto.Description;
                role.Priority = updateRoleDto.Priority;
                role.ParentRoleId = updateRoleDto.ParentRoleId;
                role.Color = updateRoleDto.Color;
                role.Icon = updateRoleDto.Icon;
                role.Category = updateRoleDto.Category;
                role.IsActive = updateRoleDto.IsActive;
                role.UpdatedAt = DateTime.UtcNow;

                var updatedRole = await _roleRepository.UpdateAsync(role);
                var roleDetail = MapToRoleDetailViewModel(updatedRole);
                return ApiResponse<RoleDetailViewModel?>.SuccessResult(roleDetail, "Role updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<RoleDetailViewModel?>.ErrorResult($"Failed to update role: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteRoleAsync(string id)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    return ApiResponse<bool>.ErrorResult("Role not found");
                }

                if (role.IsSystemRole)
                {
                    return ApiResponse<bool>.ErrorResult("Cannot delete system roles");
                }

                var result = await _roleRepository.DeleteAsync(id);
                return ApiResponse<bool>.SuccessResult(result, "Role deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to delete role: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ActivateRoleAsync(string id, bool isActive)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    return ApiResponse<bool>.ErrorResult("Role not found");
                }

                role.IsActive = isActive;
                role.UpdatedAt = DateTime.UtcNow;

                await _roleRepository.UpdateAsync(role);
                return ApiResponse<bool>.SuccessResult(true, $"Role {(isActive ? "activated" : "deactivated")} successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to update role status: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<RolePermissionViewModel>>> GetRolePermissionsAsync(string roleId)
        {
            try
            {
                var rolePermissions = await _rolePermissionRepository.GetByRoleIdAsync(roleId);
                var permissionViewModels = rolePermissions.Select(rp => new RolePermissionViewModel
                {
                    PermissionId = rp.PermissionId,
                    PermissionName = rp.Permission.Name,
                    Module = rp.Permission.Module,
                    Action = rp.Permission.Action,
                    Category = rp.Permission.Category,
                    AssignedAt = rp.AssignedAt,
                    AssignedBy = rp.AssignedBy,
                    IsActive = rp.IsActive
                }).ToList();

                return ApiResponse<List<RolePermissionViewModel>>.SuccessResult(permissionViewModels);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<RolePermissionViewModel>>.ErrorResult($"Failed to get role permissions: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> AssignPermissionToRoleAsync(string roleId, int permissionId, string assignedBy)
        {
            try
            {
                var existingAssignment = await _rolePermissionRepository.GetByRoleAndPermissionAsync(roleId, permissionId);
                if (existingAssignment != null)
                {
                    return ApiResponse<bool>.ErrorResult("Permission already assigned to role");
                }

                var rolePermission = new Models.Entities.RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId,
                    AssignedAt = DateTime.UtcNow,
                    AssignedBy = assignedBy,
                    IsActive = true
                };

                await _rolePermissionRepository.CreateAsync(rolePermission);
                return ApiResponse<bool>.SuccessResult(true, "Permission assigned to role successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to assign permission to role: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> RemovePermissionFromRoleAsync(string roleId, int permissionId)
        {
            try
            {
                var rolePermission = await _rolePermissionRepository.GetByRoleAndPermissionAsync(roleId, permissionId);
                if (rolePermission == null)
                {
                    return ApiResponse<bool>.ErrorResult("Permission not assigned to role");
                }

                var result = await _rolePermissionRepository.DeleteAsync(rolePermission);
                return ApiResponse<bool>.SuccessResult(result, "Permission removed from role successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to remove permission from role: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<RoleUserViewModel>>> GetRoleUsersAsync(string roleId)
        {
            try
            {
                var userRoles = await _userRoleRepository.GetByRoleIdAsync(roleId);
                var userViewModels = userRoles.Select(ur => new RoleUserViewModel
                {
                    UserId = ur.UserId,
                    UserName = ur.User.UserName!,
                    Email = ur.User.Email!,
                    FullName = $"{ur.User.FirstName} {ur.User.LastName}",
                    AssignedAt = ur.AssignedAt,
                    AssignedBy = ur.AssignedBy,
                    IsPrimary = ur.IsPrimary,
                    IsActive = ur.IsActive
                }).ToList();

                return ApiResponse<List<RoleUserViewModel>>.SuccessResult(userViewModels);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<RoleUserViewModel>>.ErrorResult($"Failed to get role users: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> IsNameUniqueAsync(string name, string? excludeRoleId = null)
        {
            try
            {
                var isUnique = await _roleRepository.IsNameUniqueAsync(name, excludeRoleId);
                return ApiResponse<bool>.SuccessResult(isUnique);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to check role name uniqueness: {ex.Message}");
            }
        }

        private RoleListViewModel MapToRoleListViewModel(Models.Entities.Role role)
        {
            return new RoleListViewModel
            {
                Id = role.Id,
                Name = role.Name!,
                Description = role.Description,
                Category = role.Category,
                Priority = role.Priority,
                IsActive = role.IsActive,
                IsSystemRole = role.IsSystemRole,
                CreatedAt = role.CreatedAt,
                UserCount = 0, // Would need to calculate this
                PermissionCount = 0 // Would need to calculate this
            };
        }

        private RoleDetailViewModel MapToRoleDetailViewModel(Models.Entities.Role role)
        {
            return new RoleDetailViewModel
            {
                Id = role.Id,
                Name = role.Name!,
                Description = role.Description,
                Category = role.Category,
                Priority = role.Priority,
                ParentRoleId = role.ParentRoleId,
                Color = role.Color,
                Icon = role.Icon,
                IsActive = role.IsActive,
                IsSystemRole = role.IsSystemRole,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt,
                UserCount = 0, // Would need to calculate this
                PermissionCount = 0, // Would need to calculate this
                Permissions = new List<RolePermissionViewModel>(),
                Users = new List<RoleUserViewModel>()
            };
        }
    }
}
