using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DotNet_Starter_Template.Models.DTOs.PrintJobs;
using DotNet_Starter_Template.Models.ViewModels.PrintJobs;
using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrintJobsController : ControllerBase
    {
        private readonly IPrintJobService _printJobService;
        private readonly ILogger<PrintJobsController> _logger;

        public PrintJobsController(IPrintJobService printJobService, ILogger<PrintJobsController> logger)
        {
            _printJobService = printJobService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<PrintJobListViewModel>>>> GetPrintJobs([FromQuery] PaginationRequest request)
        {
            try
            {
                var result = await _printJobService.GetAllPrintJobsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting print jobs");
                return StatusCode(500, ApiResponse<PagedResult<PrintJobListViewModel>>.ErrorResult("An error occurred while getting print jobs"));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<PrintJobDetailViewModel>>> GetPrintJob(int id)
        {
            try
            {
                var result = await _printJobService.GetPrintJobByIdAsync(id);
                if (result.Data == null)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting print job {PrintJobId}", id);
                return StatusCode(500, ApiResponse<PrintJobDetailViewModel>.ErrorResult("An error occurred while getting print job"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrintJobDetailViewModel>>> CreatePrintJob(CreatePrintJobDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(ApiResponse<PrintJobDetailViewModel>.ErrorResult("Validation failed", errors));
                }

                var requestedById = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var result = await _printJobService.CreatePrintJobAsync(createDto, requestedById);
                if (!result.Success)
                    return BadRequest(result);

                return CreatedAtAction(nameof(GetPrintJob), new { id = result.Data!.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating print job");
                return StatusCode(500, ApiResponse<PrintJobDetailViewModel>.ErrorResult("An error occurred while creating print job"));
            }
        }
    }
}
