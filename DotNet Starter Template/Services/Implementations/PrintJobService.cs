using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.PrintJobs;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Models.ViewModels.PrintJobs;
using DotNet_Starter_Template.Repositories.Interfaces;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Services.Implementations
{
    public class PrintJobService : IPrintJobService
    {
        private readonly IPrintJobRepository _printJobRepository;
        private readonly IPrinterRepository _printerRepository;
        private readonly IPrintingTemplateRepository _printingTemplateRepository;

        public PrintJobService(
            IPrintJobRepository printJobRepository,
            IPrinterRepository printerRepository,
            IPrintingTemplateRepository printingTemplateRepository)
        {
            _printJobRepository = printJobRepository;
            _printerRepository = printerRepository;
            _printingTemplateRepository = printingTemplateRepository;
        }

        public async Task<ApiResponse<PagedResult<PrintJobListViewModel>>> GetAllPrintJobsAsync(PaginationRequest request)
        {
            try
            {
                var jobs = await _printJobRepository.GetAllWithDetailsAsync();
                var viewModels = jobs.Select(MapToListViewModel).ToList();

                var totalCount = viewModels.Count;
                var pagedItems = viewModels
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var pagedResult = new PagedResult<PrintJobListViewModel>(pagedItems, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResult<PrintJobListViewModel>>.SuccessResult(pagedResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<PrintJobListViewModel>>.ErrorResult($"Failed to get print jobs: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PrintJobDetailViewModel?>> GetPrintJobByIdAsync(int id)
        {
            try
            {
                var job = await _printJobRepository.GetByIdWithDetailsAsync(id);
                if (job == null)
                    return ApiResponse<PrintJobDetailViewModel?>.ErrorResult("Print job not found");

                return ApiResponse<PrintJobDetailViewModel?>.SuccessResult(MapToDetailViewModel(job));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrintJobDetailViewModel?>.ErrorResult($"Failed to get print job: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PrintJobDetailViewModel>> CreatePrintJobAsync(CreatePrintJobDto createDto, string? requestedById = null)
        {
            try
            {
                var printer = await _printerRepository.GetByIdAsync(createDto.PrinterId);
                if (printer == null)
                    return ApiResponse<PrintJobDetailViewModel>.ErrorResult("Printer not found");

                var template = await _printingTemplateRepository.GetByIdAsync(createDto.PrintingTemplateId);
                if (template == null)
                    return ApiResponse<PrintJobDetailViewModel>.ErrorResult("Printing template not found");

                var printJob = new PrintJob
                {
                    PrinterId = createDto.PrinterId,
                    PrintingTemplateId = createDto.PrintingTemplateId,
                    RequestedById = requestedById,
                    Status = "Pending",
                    JobData = createDto.JobData,
                    Copies = createDto.Copies < 1 ? 1 : createDto.Copies,
                    CreatedAt = DateTime.UtcNow
                };

                var created = await _printJobRepository.CreateAsync(printJob);

                var createdWithDetails = await _printJobRepository.GetByIdWithDetailsAsync(created.Id);
                if (createdWithDetails == null)
                    return ApiResponse<PrintJobDetailViewModel>.ErrorResult("Failed to load created print job");

                return ApiResponse<PrintJobDetailViewModel>.SuccessResult(MapToDetailViewModel(createdWithDetails), "Print job created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PrintJobDetailViewModel>.ErrorResult($"Failed to create print job: {ex.Message}");
            }
        }

        private static PrintJobListViewModel MapToListViewModel(PrintJob j) => new()
        {
            Id = j.Id,
            PrinterId = j.PrinterId,
            PrinterName = j.Printer?.Name ?? "",
            PrintingTemplateId = j.PrintingTemplateId,
            PrintingTemplateName = j.PrintingTemplate?.Name ?? "",
            RequestedById = j.RequestedById,
            RequestedByUserName = j.RequestedBy?.UserName,
            Status = j.Status,
            Copies = j.Copies,
            CreatedAt = j.CreatedAt,
            CompletedAt = j.CompletedAt
        };

        private static PrintJobDetailViewModel MapToDetailViewModel(PrintJob j) => new()
        {
            Id = j.Id,
            PrinterId = j.PrinterId,
            PrinterName = j.Printer?.Name ?? "",
            PrintingTemplateId = j.PrintingTemplateId,
            PrintingTemplateName = j.PrintingTemplate?.Name ?? "",
            RequestedById = j.RequestedById,
            RequestedByUserName = j.RequestedBy?.UserName,
            Status = j.Status,
            JobData = j.JobData,
            Copies = j.Copies,
            CreatedAt = j.CreatedAt,
            StartedAt = j.StartedAt,
            CompletedAt = j.CompletedAt,
            ErrorMessage = j.ErrorMessage
        };
    }
}
