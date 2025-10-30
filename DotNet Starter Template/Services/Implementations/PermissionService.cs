using Microsoft.EntityFrameworkCore;
using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.Permissions;
using DotNet_Starter_Template.Models.ViewModels.Permissions;
using DotNet_Starter_Template.Repositories.Interfaces;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Services.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public PermissionService(
            IPermissionRepository permissionRepository,
            IRolePermissionRepository rolePermissionRepository)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<ApiResponse<PagedResult<PermissionListViewModel>>> GetAllPermissionsAsync(PaginationRequest request)
        {
            try
            {
                var permissions = await _permissionRepository.GetAllAsync();
                var permissionViewModels = permissions.Select(MapToPermissionListViewModel).ToList();

                // Simple pagination (in a real app, you'd do this at the database level)
                var totalCount = permissionViewModels.Count;
                var pagedItems = permissionViewModels
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var pagedResult = new PagedResult<PermissionListViewModel>(pagedItems, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResult<PermissionListViewModel>>.SuccessResult(pagedResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<PermissionListViewModel>>.ErrorResult($"Failed to get permissions: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PermissionDetailViewModel?>> GetPermissionByIdAsync(int id)
        {
            try
            {
                var permission = await _permissionRepository.GetByIdAsync(id);
                if (permission == null)
                {
                    return ApiResponse<PermissionDetailViewModel?>.ErrorResult("Permission not found");
                }

                var permissionDetail = MapToPermissionDetailViewModel(permission);
                return ApiResponse<PermissionDetailViewModel?>.SuccessResult(permissionDetail);
            }
            catch (Exception ex)
            {
                return ApiResponse<PermissionDetailViewModel?>.ErrorResult($"Failed to get permission: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PermissionDetailViewModel>> CreatePermissionAsync(CreatePermissionDto createPermissionDto)
        {
            try
            {
                // Check if name is unique
                var isUnique = await _permissionRepository.IsNameUniqueAsync(createPermissionDto.Name);
                if (!isUnique)
                {
                    return ApiResponse<PermissionDetailViewModel>.ErrorResult("Permission name already exists");
                }

                var permission = new Models.Entities.Permission
                {
                    Name = createPermissionDto.Name,
                    Description = createPermissionDto.Description,
                    Module = createPermissionDto.Module,
                    Action = createPermissionDto.Action,
                    Category = createPermissionDto.Category,
                    Group = createPermissionDto.Group,
                    SubModule = createPermissionDto.SubModule,
                    Feature = createPermissionDto.Feature,
                    Component = createPermissionDto.Component,
                    Page = createPermissionDto.Page,
                    Section = createPermissionDto.Section,
                    Level = createPermissionDto.Level,
                    Type = createPermissionDto.Type,
                    Scope = createPermissionDto.Scope,
                    Priority = createPermissionDto.Priority,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsSystemPermission = false,
                    IsDeprecated = false
                };

                var createdPermission = await _permissionRepository.CreateAsync(permission);
                var permissionDetail = MapToPermissionDetailViewModel(createdPermission);
                return ApiResponse<PermissionDetailViewModel>.SuccessResult(permissionDetail, "Permission created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PermissionDetailViewModel>.ErrorResult($"Failed to create permission: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PermissionDetailViewModel?>> UpdatePermissionAsync(int id, UpdatePermissionDto updatePermissionDto)
        {
            try
            {
                var permission = await _permissionRepository.GetByIdAsync(id);
                if (permission == null)
                {
                    return ApiResponse<PermissionDetailViewModel?>.ErrorResult("Permission not found");
                }

                // Check if name is unique (excluding current permission)
                var isUnique = await _permissionRepository.IsNameUniqueAsync(updatePermissionDto.Name, id);
                if (!isUnique)
                {
                    return ApiResponse<PermissionDetailViewModel?>.ErrorResult("Permission name already exists");
                }

                permission.Name = updatePermissionDto.Name;
                permission.Description = updatePermissionDto.Description;
                permission.Module = updatePermissionDto.Module;
                permission.Action = updatePermissionDto.Action;
                permission.Category = updatePermissionDto.Category;
                permission.Group = updatePermissionDto.Group;
                permission.SubModule = updatePermissionDto.SubModule;
                permission.Feature = updatePermissionDto.Feature;
                permission.Component = updatePermissionDto.Component;
                permission.Page = updatePermissionDto.Page;
                permission.Section = updatePermissionDto.Section;
                permission.Level = updatePermissionDto.Level;
                permission.Type = updatePermissionDto.Type;
                permission.Scope = updatePermissionDto.Scope;
                permission.Priority = updatePermissionDto.Priority;
                permission.IsActive = updatePermissionDto.IsActive;
                permission.IsDeprecated = updatePermissionDto.IsDeprecated;
                permission.ReplacementId = updatePermissionDto.ReplacementId;

                var updatedPermission = await _permissionRepository.UpdateAsync(permission);
                var permissionDetail = MapToPermissionDetailViewModel(updatedPermission);
                return ApiResponse<PermissionDetailViewModel?>.SuccessResult(permissionDetail, "Permission updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PermissionDetailViewModel?>.ErrorResult($"Failed to update permission: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeletePermissionAsync(int id)
        {
            try
            {
                var permission = await _permissionRepository.GetByIdAsync(id);
                if (permission == null)
                {
                    return ApiResponse<bool>.ErrorResult("Permission not found");
                }

                if (permission.IsSystemPermission)
                {
                    return ApiResponse<bool>.ErrorResult("Cannot delete system permissions");
                }

                var result = await _permissionRepository.DeleteAsync(id);
                return ApiResponse<bool>.SuccessResult(result, "Permission deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to delete permission: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ActivatePermissionAsync(int id, bool isActive)
        {
            try
            {
                var permission = await _permissionRepository.GetByIdAsync(id);
                if (permission == null)
                {
                    return ApiResponse<bool>.ErrorResult("Permission not found");
                }

                permission.IsActive = isActive;

                await _permissionRepository.UpdateAsync(permission);
                return ApiResponse<bool>.SuccessResult(true, $"Permission {(isActive ? "activated" : "deactivated")} successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to update permission status: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<PermissionRoleViewModel>>> GetPermissionRolesAsync(int permissionId)
        {
            try
            {
                var rolePermissions = await _rolePermissionRepository.GetByPermissionIdAsync(permissionId);
                var roleViewModels = rolePermissions.Select(rp => new PermissionRoleViewModel
                {
                    RoleId = rp.RoleId,
                    RoleName = rp.Role.Name!,
                    Description = rp.Role.Description,
                    Category = rp.Role.Category,
                    AssignedAt = rp.AssignedAt,
                    AssignedBy = rp.AssignedBy,
                    IsActive = rp.IsActive
                }).ToList();

                return ApiResponse<List<PermissionRoleViewModel>>.SuccessResult(roleViewModels);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PermissionRoleViewModel>>.ErrorResult($"Failed to get permission roles: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<string>>> GetModulesAsync()
        {
            try
            {
                var modules = await _permissionRepository.GetModulesAsync();
                return ApiResponse<List<string>>.SuccessResult(modules.ToList());
            }
            catch (Exception ex)
            {
                return ApiResponse<List<string>>.ErrorResult($"Failed to get modules: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<string>>> GetActionsAsync()
        {
            try
            {
                var actions = await _permissionRepository.GetActionsAsync();
                return ApiResponse<List<string>>.SuccessResult(actions.ToList());
            }
            catch (Exception ex)
            {
                return ApiResponse<List<string>>.ErrorResult($"Failed to get actions: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<string>>> GetCategoriesAsync()
        {
            try
            {
                var permissions = await _permissionRepository.GetAllAsync();
                var categories = permissions
                    .Where(p => !string.IsNullOrEmpty(p.Category))
                    .Select(p => p.Category!)
                    .Distinct()
                    .ToList();

                return ApiResponse<List<string>>.SuccessResult(categories);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<string>>.ErrorResult($"Failed to get categories: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> IsNameUniqueAsync(string name, int? excludePermissionId = null)
        {
            try
            {
                var isUnique = await _permissionRepository.IsNameUniqueAsync(name, excludePermissionId);
                return ApiResponse<bool>.SuccessResult(isUnique);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to check permission name uniqueness: {ex.Message}");
            }
        }

        private PermissionListViewModel MapToPermissionListViewModel(Models.Entities.Permission permission)
        {
            return new PermissionListViewModel
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                Module = permission.Module,
                Action = permission.Action,
                Category = permission.Category,
                Group = permission.Group,
                Level = permission.Level,
                Type = permission.Type,
                Scope = permission.Scope,
                Priority = permission.Priority,
                IsActive = permission.IsActive,
                IsSystemPermission = permission.IsSystemPermission,
                IsDeprecated = permission.IsDeprecated,
                CreatedAt = permission.CreatedAt,
                RoleCount = 0 // Would need to calculate this
            };
        }

        private PermissionDetailViewModel MapToPermissionDetailViewModel(Models.Entities.Permission permission)
        {
            return new PermissionDetailViewModel
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                Module = permission.Module,
                Action = permission.Action,
                Category = permission.Category,
                Group = permission.Group,
                SubModule = permission.SubModule,
                Feature = permission.Feature,
                Component = permission.Component,
                Page = permission.Page,
                Section = permission.Section,
                Level = permission.Level,
                Type = permission.Type,
                Scope = permission.Scope,
                Priority = permission.Priority,
                IsActive = permission.IsActive,
                IsSystemPermission = permission.IsSystemPermission,
                IsDeprecated = permission.IsDeprecated,
                ReplacementId = permission.ReplacementId,
                CreatedAt = permission.CreatedAt,
                RoleCount = 0, // Would need to calculate this
                Roles = new List<PermissionRoleViewModel>()
            };
        }
    }
}

