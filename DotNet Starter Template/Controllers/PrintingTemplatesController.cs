using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DotNet_Starter_Template.Models.DTOs.PrintingTemplates;
using DotNet_Starter_Template.Models.ViewModels.PrintingTemplates;
using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrintingTemplatesController : ControllerBase
    {
        private readonly IPrintingTemplateService _templateService;
        private readonly ILogger<PrintingTemplatesController> _logger;

        public PrintingTemplatesController(IPrintingTemplateService templateService, ILogger<PrintingTemplatesController> logger)
        {
            _templateService = templateService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<PrintingTemplateListViewModel>>>> GetTemplates([FromQuery] PaginationRequest request)
        {
            try
            {
                var result = await _templateService.GetAllTemplatesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting printing templates");
                return StatusCode(500, ApiResponse<PagedResult<PrintingTemplateListViewModel>>.ErrorResult("An error occurred while getting templates"));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<PrintingTemplateDetailViewModel>>> GetTemplate(int id)
        {
            try
            {
                var result = await _templateService.GetTemplateByIdAsync(id);
                if (result.Data == null)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting template {TemplateId}", id);
                return StatusCode(500, ApiResponse<PrintingTemplateDetailViewModel>.ErrorResult("An error occurred while getting template"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrintingTemplateDetailViewModel>>> CreateTemplate(CreatePrintingTemplateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(ApiResponse<PrintingTemplateDetailViewModel>.ErrorResult("Validation failed", errors));
                }

                var result = await _templateService.CreateTemplateAsync(createDto);
                if (!result.Success)
                    return BadRequest(result);

                return CreatedAtAction(nameof(GetTemplate), new { id = result.Data!.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating printing template");
                return StatusCode(500, ApiResponse<PrintingTemplateDetailViewModel>.ErrorResult("An error occurred while creating template"));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<PrintingTemplateDetailViewModel>>> UpdateTemplate(int id, UpdatePrintingTemplateDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(ApiResponse<PrintingTemplateDetailViewModel>.ErrorResult("Validation failed", errors));
                }

                var result = await _templateService.UpdateTemplateAsync(id, updateDto);
                if (result.Data == null)
                    return NotFound(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating template {TemplateId}", id);
                return StatusCode(500, ApiResponse<PrintingTemplateDetailViewModel>.ErrorResult("An error occurred while updating template"));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteTemplate(int id)
        {
            try
            {
                var result = await _templateService.DeleteTemplateAsync(id);
                if (!result.Success)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting template {TemplateId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while deleting template"));
            }
        }

        [HttpPut("{id:int}/activate")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivateTemplate(int id, [FromBody] bool isActive)
        {
            try
            {
                var result = await _templateService.ActivateTemplateAsync(id, isActive);
                if (!result.Success)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating/deactivating template {TemplateId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while updating template status"));
            }
        }

        [HttpGet("check-name")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckTemplateName([FromQuery] string name, [FromQuery] int? excludeId = null)
        {
            try
            {
                var result = await _templateService.IsNameUniqueAsync(name, excludeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking template name uniqueness");
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while checking template name"));
            }
        }
    }
}
