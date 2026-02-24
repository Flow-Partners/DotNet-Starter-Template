using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DotNet_Starter_Template.Models.DTOs.Printers;
using DotNet_Starter_Template.Models.ViewModels.Printers;
using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrintersController : ControllerBase
    {
        private readonly IPrinterService _printerService;
        private readonly ILogger<PrintersController> _logger;

        public PrintersController(IPrinterService printerService, ILogger<PrintersController> logger)
        {
            _printerService = printerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<PrinterListViewModel>>>> GetPrinters([FromQuery] PaginationRequest request)
        {
            try
            {
                var result = await _printerService.GetAllPrintersAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting printers");
                return StatusCode(500, ApiResponse<PagedResult<PrinterListViewModel>>.ErrorResult("An error occurred while getting printers"));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<PrinterDetailViewModel>>> GetPrinter(int id)
        {
            try
            {
                var result = await _printerService.GetPrinterByIdAsync(id);
                if (result.Data == null)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting printer {PrinterId}", id);
                return StatusCode(500, ApiResponse<PrinterDetailViewModel>.ErrorResult("An error occurred while getting printer"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrinterDetailViewModel>>> CreatePrinter(CreatePrinterDto createPrinterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(ApiResponse<PrinterDetailViewModel>.ErrorResult("Validation failed", errors));
                }

                var result = await _printerService.CreatePrinterAsync(createPrinterDto);
                if (!result.Success)
                    return BadRequest(result);

                return CreatedAtAction(nameof(GetPrinter), new { id = result.Data!.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating printer");
                return StatusCode(500, ApiResponse<PrinterDetailViewModel>.ErrorResult("An error occurred while creating printer"));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<PrinterDetailViewModel>>> UpdatePrinter(int id, UpdatePrinterDto updatePrinterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(ApiResponse<PrinterDetailViewModel>.ErrorResult("Validation failed", errors));
                }

                var result = await _printerService.UpdatePrinterAsync(id, updatePrinterDto);
                if (result.Data == null)
                    return NotFound(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating printer {PrinterId}", id);
                return StatusCode(500, ApiResponse<PrinterDetailViewModel>.ErrorResult("An error occurred while updating printer"));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePrinter(int id)
        {
            try
            {
                var result = await _printerService.DeletePrinterAsync(id);
                if (!result.Success)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting printer {PrinterId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while deleting printer"));
            }
        }

        [HttpPut("{id:int}/activate")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivatePrinter(int id, [FromBody] bool isActive)
        {
            try
            {
                var result = await _printerService.ActivatePrinterAsync(id, isActive);
                if (!result.Success)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating/deactivating printer {PrinterId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while updating printer status"));
            }
        }

        [HttpGet("check-name")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckPrinterName([FromQuery] string name, [FromQuery] int? excludeId = null)
        {
            try
            {
                var result = await _printerService.IsNameUniqueAsync(name, excludeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking printer name uniqueness");
                return StatusCode(500, ApiResponse<bool>.ErrorResult("An error occurred while checking printer name"));
            }
        }
    }
}
