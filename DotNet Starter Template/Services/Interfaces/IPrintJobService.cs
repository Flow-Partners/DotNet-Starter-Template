using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.PrintJobs;
using DotNet_Starter_Template.Models.ViewModels.PrintJobs;

namespace DotNet_Starter_Template.Services.Interfaces
{
    public interface IPrintJobService
    {
        Task<ApiResponse<PagedResult<PrintJobListViewModel>>> GetAllPrintJobsAsync(PaginationRequest request);
        Task<ApiResponse<PrintJobDetailViewModel?>> GetPrintJobByIdAsync(int id);
        Task<ApiResponse<PrintJobDetailViewModel>> CreatePrintJobAsync(CreatePrintJobDto createDto, string? requestedById = null);
    }
}
