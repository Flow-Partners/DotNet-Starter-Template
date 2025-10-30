using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DotNet_Starter_Template.Models.DTOs.Roles;
using DotNet_Starter_Template.Models.ViewModels.Roles;
using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<RoleListViewModel>>>> GetRoles([FromQuery] PaginationRequest request)
        {
            try
            {
                var result = await _roleService.GetAllRolesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles");
                return StatusCode(500, ApiResponse<PagedResult<RoleListViewModel>>.ErrorResult("An error occurred while getting roles"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<RoleDetailViewModel>>> GetRole(string id)
        {
            try
            {
                var result = await _roleService.GetRoleByIdAsync(id);
                if (result.Data == null)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting role {RoleId}", id);
                return StatusCode(500, ApiResponse<RoleDetailViewModel>.ErrorResult("An error occurred while getting role"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<RoleDetailViewModel>>> CreateRole(CreateRoleDto createRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(ApiResponse<RoleDetailViewModel>.ErrorResult("Validation failed", errors));
                }

                var result = await _roleService.CreateRoleAsync(createRoleDto);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return CreatedAtAction(nameof(GetRole), new { id = result.Data!.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                return StatusCode(500, ApiResponse<RoleDetailViewModel>.ErrorResult("An error occurred while creating role"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<RoleDetailViewModel>>> UpdateRole(string id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(ApiResponse<RoleDetailViewModel>.ErrorResult("Validation failed", errors));
                }

                var result = await _roleService.UpdateRoleAsync(id, updateRoleDto);
                if (result.Data == null)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role {RoleId}", id);
                return StatusCode(500, ApiResponse<RoleDetailViewModel>.ErrorResult("An error occurred while updating role"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteRole(string id)
        {
            try
            {
                var result = await _roleService.DeleteRoleAsync(id);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role {RoleId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while deleting role"));
            }
        }

        [HttpPut("{id}/activate")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivateRole(string id, [FromBody] bool isActive)
        {
            try
            {
                var result = await _roleService.ActivateRoleAsync(id, isActive);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating/deactivating role {RoleId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while updating role status"));
            }
        }

        [HttpGet("{id}/permissions")]
        public async Task<ActionResult<ApiResponse<List<RolePermissionViewModel>>>> GetRolePermissions(string id)
        {
            try
            {
                var result = await _roleService.GetRolePermissionsAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting role permissions for role {RoleId}", id);
                return StatusCode(500, ApiResponse<List<RolePermissionViewModel>>.ErrorResult("An error occurred while getting role permissions"));
            }
        }

        [HttpPost("{id}/permissions/{permissionId}")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignPermissionToRole(string id, int permissionId)
        {
            try
            {
                var assignedBy = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "System";
                var result = await _roleService.AssignPermissionToRoleAsync(id, permissionId, assignedBy);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning permission {PermissionId} to role {RoleId}", permissionId, id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while assigning permission to role"));
            }
        }

        [HttpDelete("{id}/permissions/{permissionId}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemovePermissionFromRole(string id, int permissionId)
        {
            try
            {
                var result = await _roleService.RemovePermissionFromRoleAsync(id, permissionId);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing permission {PermissionId} from role {RoleId}", permissionId, id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while removing permission from role"));
            }
        }

        [HttpGet("{id}/users")]
        public async Task<ActionResult<ApiResponse<List<RoleUserViewModel>>>> GetRoleUsers(string id)
        {
            try
            {
                var result = await _roleService.GetRoleUsersAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users for role {RoleId}", id);
                return StatusCode(500, ApiResponse<List<RoleUserViewModel>>.ErrorResult("An error occurred while getting role users"));
            }
        }

        [HttpGet("check-name")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckRoleName([FromQuery] string name, [FromQuery] string? excludeId = null)
        {
            try
            {
                var result = await _roleService.IsNameUniqueAsync(name, excludeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking role name uniqueness");
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while checking role name"));
            }
        }
    }
}

