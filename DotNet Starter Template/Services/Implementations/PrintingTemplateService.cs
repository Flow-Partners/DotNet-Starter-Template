using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.PrintingTemplates;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Models.ViewModels.PrintingTemplates;
using DotNet_Starter_Template.Repositories.Interfaces;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Services.Implementations
{
    public class PrintingTemplateService : IPrintingTemplateService
    {
        private readonly IPrintingTemplateRepository _templateRepository;

        public PrintingTemplateService(IPrintingTemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task<ApiResponse<PagedResult<PrintingTemplateListViewModel>>> GetAllTemplatesAsync(PaginationRequest request)
        {
            try
            {
                var templates = await _templateRepository.GetAllAsync();
                var viewModels = templates.Select(MapToListViewModel).ToList();

                var totalCount = viewModels.Count;
                var pagedItems = viewModels
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var pagedResult = new PagedResult<PrintingTemplateListViewModel>(pagedItems, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResult<PrintingTemplateListViewModel>>.SuccessResult(pagedResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<PrintingTemplateListViewModel>>.ErrorResult($"Failed to get templates: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PrintingTemplateDetailViewModel?>> GetTemplateByIdAsync(int id)
        {
            try
            {
                var template = await _templateRepository.GetByIdAsync(id);
                if (template == null)
                    return ApiResponse<PrintingTemplateDetailViewModel?>.ErrorResult("Template not found");

                return ApiResponse<PrintingTemplateDetailViewModel?>.SuccessResult(MapToDetailViewModel(template));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrintingTemplateDetailViewModel?>.ErrorResult($"Failed to get template: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PrintingTemplateDetailViewModel>> CreateTemplateAsync(CreatePrintingTemplateDto createDto)
        {
            try
            {
                var isUnique = await _templateRepository.IsNameUniqueAsync(createDto.Name);
                if (!isUnique)
                    return ApiResponse<PrintingTemplateDetailViewModel>.ErrorResult("Template name already exists");

                var template = new PrintingTemplate
                {
                    Name = createDto.Name,
                    Description = createDto.Description,
                    Content = createDto.Content,
                    TemplateType = createDto.TemplateType,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var created = await _templateRepository.CreateAsync(template);
                return ApiResponse<PrintingTemplateDetailViewModel>.SuccessResult(MapToDetailViewModel(created), "Template created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PrintingTemplateDetailViewModel>.ErrorResult($"Failed to create template: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PrintingTemplateDetailViewModel?>> UpdateTemplateAsync(int id, UpdatePrintingTemplateDto updateDto)
        {
            try
            {
                var template = await _templateRepository.GetByIdAsync(id);
                if (template == null)
                    return ApiResponse<PrintingTemplateDetailViewModel?>.ErrorResult("Template not found");

                var isUnique = await _templateRepository.IsNameUniqueAsync(updateDto.Name, id);
                if (!isUnique)
                    return ApiResponse<PrintingTemplateDetailViewModel?>.ErrorResult("Template name already exists");

                template.Name = updateDto.Name;
                template.Description = updateDto.Description;
                template.Content = updateDto.Content;
                template.TemplateType = updateDto.TemplateType;
                template.IsActive = updateDto.IsActive;
                template.UpdatedAt = DateTime.UtcNow;

                var updated = await _templateRepository.UpdateAsync(template);
                return ApiResponse<PrintingTemplateDetailViewModel?>.SuccessResult(MapToDetailViewModel(updated), "Template updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PrintingTemplateDetailViewModel?>.ErrorResult($"Failed to update template: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteTemplateAsync(int id)
        {
            try
            {
                var template = await _templateRepository.GetByIdAsync(id);
                if (template == null)
                    return ApiResponse<bool>.ErrorResult("Template not found");

                var result = await _templateRepository.DeleteAsync(id);
                return ApiResponse<bool>.SuccessResult(result, "Template deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to delete template: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ActivateTemplateAsync(int id, bool isActive)
        {
            try
            {
                var template = await _templateRepository.GetByIdAsync(id);
                if (template == null)
                    return ApiResponse<bool>.ErrorResult("Template not found");

                template.IsActive = isActive;
                template.UpdatedAt = DateTime.UtcNow;
                await _templateRepository.UpdateAsync(template);
                return ApiResponse<bool>.SuccessResult(true, $"Template {(isActive ? "activated" : "deactivated")} successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to update template status: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> IsNameUniqueAsync(string name, int? excludeTemplateId = null)
        {
            try
            {
                var isUnique = await _templateRepository.IsNameUniqueAsync(name, excludeTemplateId);
                return ApiResponse<bool>.SuccessResult(isUnique);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to check template name: {ex.Message}");
            }
        }

        private static PrintingTemplateListViewModel MapToListViewModel(PrintingTemplate t) => new()
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            TemplateType = t.TemplateType,
            IsActive = t.IsActive,
            CreatedAt = t.CreatedAt
        };

        private static PrintingTemplateDetailViewModel MapToDetailViewModel(PrintingTemplate t) => new()
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            Content = t.Content,
            TemplateType = t.TemplateType,
            IsActive = t.IsActive,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        };
    }
}
