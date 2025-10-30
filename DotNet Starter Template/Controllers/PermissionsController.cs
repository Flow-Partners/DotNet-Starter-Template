using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DotNet_Starter_Template.Models.DTOs.Permissions;
using DotNet_Starter_Template.Models.ViewModels.Permissions;
using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(IPermissionService permissionService, ILogger<PermissionsController> logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<PermissionListViewModel>>>> GetPermissions([FromQuery] PaginationRequest request)
        {
            try
            {
                var result = await _permissionService.GetAllPermissionsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permissions");
                return StatusCode(500, ApiResponse<PagedResult<PermissionListViewModel>>.ErrorResult("An error occurred while getting permissions"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PermissionDetailViewModel>>> GetPermission(int id)
        {
            try
            {
                var result = await _permissionService.GetPermissionByIdAsync(id);
                if (result.Data == null)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permission {PermissionId}", id);
                return StatusCode(500, ApiResponse<PermissionDetailViewModel>.ErrorResult("An error occurred while getting permission"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PermissionDetailViewModel>>> CreatePermission(CreatePermissionDto createPermissionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(ApiResponse<PermissionDetailViewModel>.ErrorResult("Validation failed", errors));
                }

                var result = await _permissionService.CreatePermissionAsync(createPermissionDto);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return CreatedAtAction(nameof(GetPermission), new { id = result.Data!.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating permission");
                return StatusCode(500, ApiResponse<PermissionDetailViewModel>.ErrorResult("An error occurred while creating permission"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PermissionDetailViewModel>>> UpdatePermission(int id, UpdatePermissionDto updatePermissionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(ApiResponse<PermissionDetailViewModel>.ErrorResult("Validation failed", errors));
                }

                var result = await _permissionService.UpdatePermissionAsync(id, updatePermissionDto);
                if (result.Data == null)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating permission {PermissionId}", id);
                return StatusCode(500, ApiResponse<PermissionDetailViewModel>.ErrorResult("An error occurred while updating permission"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePermission(int id)
        {
            try
            {
                var result = await _permissionService.DeletePermissionAsync(id);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting permission {PermissionId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while deleting permission"));
            }
        }

        [HttpPut("{id}/activate")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivatePermission(int id, [FromBody] bool isActive)
        {
            try
            {
                var result = await _permissionService.ActivatePermissionAsync(id, isActive);
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating/deactivating permission {PermissionId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while updating permission status"));
            }
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult<ApiResponse<List<PermissionRoleViewModel>>>> GetPermissionRoles(int id)
        {
            try
            {
                var result = await _permissionService.GetPermissionRolesAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles for permission {PermissionId}", id);
                return StatusCode(500, ApiResponse<List<PermissionRoleViewModel>>.ErrorResult("An error occurred while getting permission roles"));
            }
        }

        [HttpGet("modules")]
        public async Task<ActionResult<ApiResponse<List<string>>>> GetModules()
        {
            try
            {
                var result = await _permissionService.GetModulesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting modules");
                return StatusCode(500, ApiResponse<List<string>>.ErrorResult("An error occurred while getting modules"));
            }
        }

        [HttpGet("actions")]
        public async Task<ActionResult<ApiResponse<List<string>>>> GetActions()
        {
            try
            {
                var result = await _permissionService.GetActionsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting actions");
                return StatusCode(500, ApiResponse<List<string>>.ErrorResult("An error occurred while getting actions"));
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult<ApiResponse<List<string>>>> GetCategories()
        {
            try
            {
                var result = await _permissionService.GetCategoriesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(500, ApiResponse<List<string>>.ErrorResult("An error occurred while getting categories"));
            }
        }

        [HttpGet("check-name")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckPermissionName([FromQuery] string name, [FromQuery] int? excludeId = null)
        {
            try
            {
                var result = await _permissionService.IsNameUniqueAsync(name, excludeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking permission name uniqueness");
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while checking permission name"));
            }
        }
    }
}

