using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.Users;
using DotNet_Starter_Template.Models.ViewModels.Users;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Repositories.Interfaces;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserService(
            UserManager<User> userManager,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<ApiResponse<PagedResult<UserListViewModel>>> GetAllUsersAsync(PaginationRequest request)
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                var userViewModels = new List<UserListViewModel>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userViewModels.Add(MapToUserListViewModel(user, roles.ToList()));
                }

                // Simple pagination (in a real app, you'd do this at the database level)
                var totalCount = userViewModels.Count;
                var pagedItems = userViewModels
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var pagedResult = new PagedResult<UserListViewModel>(pagedItems, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResult<UserListViewModel>>.SuccessResult(pagedResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<UserListViewModel>>.ErrorResult($"Failed to get users: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserDetailViewModel?>> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return ApiResponse<UserDetailViewModel?>.ErrorResult("User not found");
                }

                var roles = await _userManager.GetRolesAsync(user);
                var userRoles = await _userRoleRepository.GetByUserIdAsync(id);
                var permissions = new List<string>(); // Placeholder for actual permission retrieval

                var userDetail = MapToUserDetailViewModel(user, roles.ToList(), userRoles.ToList(), permissions);
                return ApiResponse<UserDetailViewModel?>.SuccessResult(userDetail);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDetailViewModel?>.ErrorResult($"Failed to get user: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserDetailViewModel>> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                // Check if email is unique
                var existingUser = await _userManager.FindByEmailAsync(createUserDto.Email);
                if (existingUser != null)
                {
                    return ApiResponse<UserDetailViewModel>.ErrorResult("User with this email already exists");
                }

                var user = new User
                {
                    UserName = createUserDto.Email,
                    Email = createUserDto.Email,
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    PhoneNumber = createUserDto.PhoneNumber,
                    DateOfBirth = createUserDto.DateOfBirth,
                    Gender = createUserDto.Gender,
                    Address = createUserDto.Address,
                    City = createUserDto.City,
                    State = createUserDto.State,
                    Country = createUserDto.Country,
                    PostalCode = createUserDto.PostalCode,
                    Timezone = createUserDto.Timezone,
                    Language = createUserDto.Language,
                    Theme = createUserDto.Theme,
                    ProfilePicture = createUserDto.ProfilePicture,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false
                };

                var result = await _userManager.CreateAsync(user, createUserDto.Password);
                if (!result.Succeeded)
                {
                    return ApiResponse<UserDetailViewModel>.ErrorResult("Failed to create user", result.Errors.Select(e => e.Description).ToList());
                }

                // Assign default role if specified
                if (!string.IsNullOrEmpty(createUserDto.DefaultRoleId))
                {
                    var role = await _roleRepository.GetByIdAsync(createUserDto.DefaultRoleId);
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name!);
                    }
                }

                var userDetail = MapToUserDetailViewModel(user, new List<string>(), new List<UserRole>(), new List<string>());
                return ApiResponse<UserDetailViewModel>.SuccessResult(userDetail, "User created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDetailViewModel>.ErrorResult($"Failed to create user: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserDetailViewModel?>> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return ApiResponse<UserDetailViewModel?>.ErrorResult("User not found");
                }

                // Check if email is unique (excluding current user)
                if (user.Email != updateUserDto.Email)
                {
                    var existingUser = await _userManager.FindByEmailAsync(updateUserDto.Email);
                    if (existingUser != null)
                    {
                        return ApiResponse<UserDetailViewModel?>.ErrorResult("User with this email already exists");
                    }
                }

                user.FirstName = updateUserDto.FirstName;
                user.LastName = updateUserDto.LastName;
                user.Email = updateUserDto.Email;
                user.UserName = updateUserDto.Email;
                user.PhoneNumber = updateUserDto.PhoneNumber;
                user.DateOfBirth = updateUserDto.DateOfBirth;
                user.Gender = updateUserDto.Gender;
                user.Address = updateUserDto.Address;
                user.City = updateUserDto.City;
                user.State = updateUserDto.State;
                user.Country = updateUserDto.Country;
                user.PostalCode = updateUserDto.PostalCode;
                user.Timezone = updateUserDto.Timezone;
                user.Language = updateUserDto.Language;
                user.Theme = updateUserDto.Theme;
                user.ProfilePicture = updateUserDto.ProfilePicture;
                user.IsActive = updateUserDto.IsActive;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return ApiResponse<UserDetailViewModel?>.ErrorResult("Failed to update user", result.Errors.Select(e => e.Description).ToList());
                }

                var roles = await _userManager.GetRolesAsync(user);
                var userRoles = await _userRoleRepository.GetByUserIdAsync(id);
                var permissions = new List<string>(); // Placeholder

                var userDetail = MapToUserDetailViewModel(user, roles.ToList(), userRoles.ToList(), permissions);
                return ApiResponse<UserDetailViewModel?>.SuccessResult(userDetail, "User updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDetailViewModel?>.ErrorResult($"Failed to update user: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return ApiResponse<bool>.ErrorResult("User not found");
                }

                // Don't allow deletion of super admin
                if (user.Email == "superadmin@example.com")
                {
                    return ApiResponse<bool>.ErrorResult("Cannot delete super admin user");
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return ApiResponse<bool>.ErrorResult("Failed to delete user", result.Errors.Select(e => e.Description).ToList());
                }

                return ApiResponse<bool>.SuccessResult(true, "User deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to delete user: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ActivateUserAsync(string id, bool isActive)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return ApiResponse<bool>.ErrorResult("User not found");
                }

                user.IsActive = isActive;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return ApiResponse<bool>.ErrorResult("Failed to update user status", result.Errors.Select(e => e.Description).ToList());
                }

                return ApiResponse<bool>.SuccessResult(true, $"User {(isActive ? "activated" : "deactivated")} successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to update user status: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ResetUserPasswordAsync(string id, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return ApiResponse<bool>.ErrorResult("User not found");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (!result.Succeeded)
                {
                    return ApiResponse<bool>.ErrorResult("Failed to reset password", result.Errors.Select(e => e.Description).ToList());
                }

                return ApiResponse<bool>.SuccessResult(true, "Password reset successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to reset password: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<UserRoleViewModel>>> GetUserRolesAsync(string userId)
        {
            try
            {
                var userRoles = await _userRoleRepository.GetByUserIdAsync(userId);
                var roleViewModels = userRoles.Select(ur => new UserRoleViewModel
                {
                    RoleId = ur.RoleId,
                    RoleName = ur.Role.Name!,
                    Description = ur.Role.Description,
                    AssignedAt = ur.AssignedAt,
                    AssignedBy = ur.AssignedBy,
                    IsPrimary = ur.IsPrimary,
                    IsActive = ur.IsActive
                }).ToList();

                return ApiResponse<List<UserRoleViewModel>>.SuccessResult(roleViewModels);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UserRoleViewModel>>.ErrorResult($"Failed to get user roles: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> AssignRoleToUserAsync(string userId, string roleId, string assignedBy)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.ErrorResult("User not found");
                }

                var role = await _roleRepository.GetByIdAsync(roleId);
                if (role == null)
                {
                    return ApiResponse<bool>.ErrorResult("Role not found");
                }

                var result = await _userManager.AddToRoleAsync(user, role.Name!);
                if (!result.Succeeded)
                {
                    return ApiResponse<bool>.ErrorResult("Failed to assign role to user", result.Errors.Select(e => e.Description).ToList());
                }

                return ApiResponse<bool>.SuccessResult(true, "Role assigned to user successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to assign role to user: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> RemoveRoleFromUserAsync(string userId, string roleId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.ErrorResult("User not found");
                }

                var role = await _roleRepository.GetByIdAsync(roleId);
                if (role == null)
                {
                    return ApiResponse<bool>.ErrorResult("Role not found");
                }

                var result = await _userManager.RemoveFromRoleAsync(user, role.Name!);
                if (!result.Succeeded)
                {
                    return ApiResponse<bool>.ErrorResult("Failed to remove role from user", result.Errors.Select(e => e.Description).ToList());
                }

                return ApiResponse<bool>.SuccessResult(true, "Role removed from user successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to remove role from user: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<string>>> GetUserPermissionsAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<List<string>>.ErrorResult("User not found");
                }

                var roles = await _userManager.GetRolesAsync(user);
                var permissions = new List<string>(); // Placeholder - would need to implement actual permission retrieval

                return ApiResponse<List<string>>.SuccessResult(permissions);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<string>>.ErrorResult($"Failed to get user permissions: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> IsEmailUniqueAsync(string email, string? excludeUserId = null)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    return ApiResponse<bool>.SuccessResult(true);
                }

                if (!string.IsNullOrEmpty(excludeUserId) && existingUser.Id == excludeUserId)
                {
                    return ApiResponse<bool>.SuccessResult(true);
                }

                return ApiResponse<bool>.SuccessResult(false);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to check email uniqueness: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> IsUserNameUniqueAsync(string userName, string? excludeUserId = null)
        {
            try
            {
                var existingUser = await _userManager.FindByNameAsync(userName);
                if (existingUser == null)
                {
                    return ApiResponse<bool>.SuccessResult(true);
                }

                if (!string.IsNullOrEmpty(excludeUserId) && existingUser.Id == excludeUserId)
                {
                    return ApiResponse<bool>.SuccessResult(true);
                }

                return ApiResponse<bool>.SuccessResult(false);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to check username uniqueness: {ex.Message}");
            }
        }

        private UserListViewModel MapToUserListViewModel(User user, List<string> roles)
        {
            return new UserListViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                UserName = user.UserName!,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive,
                Roles = roles
            };
        }

        private UserDetailViewModel MapToUserDetailViewModel(User user, List<string> roles, List<UserRole> userRoles, List<string> permissions)
        {
            return new UserDetailViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                UserName = user.UserName!,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                City = user.City,
                State = user.State,
                Country = user.Country,
                PostalCode = user.PostalCode,
                Timezone = user.Timezone,
                Language = user.Language,
                Theme = user.Theme,
                ProfilePicture = user.ProfilePicture,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                Roles = userRoles.Select(ur => new UserRoleViewModel
                {
                    RoleId = ur.RoleId,
                    RoleName = ur.Role.Name!,
                    Description = ur.Role.Description,
                    AssignedAt = ur.AssignedAt,
                    AssignedBy = ur.AssignedBy,
                    IsPrimary = ur.IsPrimary,
                    IsActive = ur.IsActive
                }).ToList(),
                Permissions = permissions
            };
        }
    }
}
