using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DotNet_Starter_Template.Models.DTOs.Users;
using DotNet_Starter_Template.Models.ViewModels.Users;
using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<UserListViewModel>>>> GetUsers([FromQuery] PaginationRequest request)
        {
            try
            {
                var result = await _userService.GetAllUsersAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, ApiResponse<PagedResult<UserListViewModel>>.ErrorResult("An error occurred while getting users"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDetailViewModel>>> GetUser(string id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);
                if (result.Data == null)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {UserId}", id);
                return StatusCode(500, ApiResponse<UserDetailViewModel>.ErrorResult("An error occurred while getting user"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserDetailViewModel>>> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(ApiResponse<UserDetailViewModel>.ErrorResult("Validation failed", errors));
                }

                var result = await _userService.CreateUserAsync(createUserDto);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return CreatedAtAction(nameof(GetUser), new { id = result.Data!.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, ApiResponse<UserDetailViewModel>.ErrorResult("An error occurred while creating user"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserDetailViewModel>>> UpdateUser(string id, UpdateUserDto updateUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(ApiResponse<UserDetailViewModel>.ErrorResult("Validation failed", errors));
                }

                var result = await _userService.UpdateUserAsync(id, updateUserDto);
                if (result.Data == null)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                return StatusCode(500, ApiResponse<UserDetailViewModel>.ErrorResult("An error occurred while updating user"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(string id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while deleting user"));
            }
        }

        [HttpPut("{id}/activate")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivateUser(string id, [FromBody] bool isActive)
        {
            try
            {
                var result = await _userService.ActivateUserAsync(id, isActive);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating/deactivating user {UserId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while updating user status"));
            }
        }

        [HttpPost("{id}/reset-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ResetUserPassword(string id, [FromBody] string newPassword)
        {
            try
            {
                var result = await _userService.ResetUserPasswordAsync(id, newPassword);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for user {UserId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while resetting password"));
            }
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult<ApiResponse<List<UserRoleViewModel>>>> GetUserRoles(string id)
        {
            try
            {
                var result = await _userService.GetUserRolesAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles for user {UserId}", id);
                return StatusCode(500, ApiResponse<List<UserRoleViewModel>>.ErrorResult("An error occurred while getting user roles"));
            }
        }

        [HttpPost("{id}/roles/{roleId}")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignRoleToUser(string id, string roleId)
        {
            try
            {
                var assignedBy = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "System";
                var result = await _userService.AssignRoleToUserAsync(id, roleId, assignedBy);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role {RoleId} to user {UserId}", roleId, id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while assigning role to user"));
            }
        }

        [HttpDelete("{id}/roles/{roleId}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveRoleFromUser(string id, string roleId)
        {
            try
            {
                var result = await _userService.RemoveRoleFromUserAsync(id, roleId);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing role {RoleId} from user {UserId}", roleId, id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while removing role from user"));
            }
        }

        [HttpGet("{id}/permissions")]
        public async Task<ActionResult<ApiResponse<List<string>>>> GetUserPermissions(string id)
        {
            try
            {
                var result = await _userService.GetUserPermissionsAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permissions for user {UserId}", id);
                return StatusCode(500, ApiResponse<List<string>>.ErrorResult("An error occurred while getting user permissions"));
            }
        }

        [HttpGet("check-email")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckEmail([FromQuery] string email, [FromQuery] string? excludeId = null)
        {
            try
            {
                var result = await _userService.IsEmailUniqueAsync(email, excludeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email uniqueness");
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while checking email"));
            }
        }

        [HttpGet("check-username")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckUsername([FromQuery] string userName, [FromQuery] string? excludeId = null)
        {
            try
            {
                var result = await _userService.IsUserNameUniqueAsync(userName, excludeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking username uniqueness");
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while checking username"));
            }
        }
    }
}

