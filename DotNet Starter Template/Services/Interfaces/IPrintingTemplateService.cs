using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.PrintingTemplates;
using DotNet_Starter_Template.Models.ViewModels.PrintingTemplates;

namespace DotNet_Starter_Template.Services.Interfaces
{
    public interface IPrintingTemplateService
    {
        Task<ApiResponse<PagedResult<PrintingTemplateListViewModel>>> GetAllTemplatesAsync(PaginationRequest request);
        Task<ApiResponse<PrintingTemplateDetailViewModel?>> GetTemplateByIdAsync(int id);
        Task<ApiResponse<PrintingTemplateDetailViewModel>> CreateTemplateAsync(CreatePrintingTemplateDto createDto);
        Task<ApiResponse<PrintingTemplateDetailViewModel?>> UpdateTemplateAsync(int id, UpdatePrintingTemplateDto updateDto);
        Task<ApiResponse<bool>> DeleteTemplateAsync(int id);
        Task<ApiResponse<bool>> ActivateTemplateAsync(int id, bool isActive);
        Task<ApiResponse<bool>> IsNameUniqueAsync(string name, int? excludeTemplateId = null);
    }
}
