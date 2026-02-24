using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.Printers;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Models.ViewModels.Printers;
using DotNet_Starter_Template.Repositories.Interfaces;
using DotNet_Starter_Template.Services.Interfaces;

namespace DotNet_Starter_Template.Services.Implementations
{
    public class PrinterService : IPrinterService
    {
        private readonly IPrinterRepository _printerRepository;

        public PrinterService(IPrinterRepository printerRepository)
        {
            _printerRepository = printerRepository;
        }

        public async Task<ApiResponse<PagedResult<PrinterListViewModel>>> GetAllPrintersAsync(PaginationRequest request)
        {
            try
            {
                var printers = await _printerRepository.GetAllAsync();
                var viewModels = printers.Select(MapToListViewModel).ToList();

                var totalCount = viewModels.Count;
                var pagedItems = viewModels
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var pagedResult = new PagedResult<PrinterListViewModel>(pagedItems, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResult<PrinterListViewModel>>.SuccessResult(pagedResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<PrinterListViewModel>>.ErrorResult($"Failed to get printers: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PrinterDetailViewModel?>> GetPrinterByIdAsync(int id)
        {
            try
            {
                var printer = await _printerRepository.GetByIdAsync(id);
                if (printer == null)
                    return ApiResponse<PrinterDetailViewModel?>.ErrorResult("Printer not found");

                return ApiResponse<PrinterDetailViewModel?>.SuccessResult(MapToDetailViewModel(printer));
            }
            catch (Exception ex)
            {
                return ApiResponse<PrinterDetailViewModel?>.ErrorResult($"Failed to get printer: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PrinterDetailViewModel>> CreatePrinterAsync(CreatePrinterDto createPrinterDto)
        {
            try
            {
                var isUnique = await _printerRepository.IsNameUniqueAsync(createPrinterDto.Name);
                if (!isUnique)
                    return ApiResponse<PrinterDetailViewModel>.ErrorResult("Printer name already exists");

                var printer = new Printer
                {
                    Name = createPrinterDto.Name,
                    Description = createPrinterDto.Description,
                    ConnectionInfo = createPrinterDto.ConnectionInfo,
                    PrinterType = createPrinterDto.PrinterType,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var created = await _printerRepository.CreateAsync(printer);
                return ApiResponse<PrinterDetailViewModel>.SuccessResult(MapToDetailViewModel(created), "Printer created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PrinterDetailViewModel>.ErrorResult($"Failed to create printer: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PrinterDetailViewModel?>> UpdatePrinterAsync(int id, UpdatePrinterDto updatePrinterDto)
        {
            try
            {
                var printer = await _printerRepository.GetByIdAsync(id);
                if (printer == null)
                    return ApiResponse<PrinterDetailViewModel?>.ErrorResult("Printer not found");

                var isUnique = await _printerRepository.IsNameUniqueAsync(updatePrinterDto.Name, id);
                if (!isUnique)
                    return ApiResponse<PrinterDetailViewModel?>.ErrorResult("Printer name already exists");

                printer.Name = updatePrinterDto.Name;
                printer.Description = updatePrinterDto.Description;
                printer.ConnectionInfo = updatePrinterDto.ConnectionInfo;
                printer.PrinterType = updatePrinterDto.PrinterType;
                printer.IsActive = updatePrinterDto.IsActive;
                printer.UpdatedAt = DateTime.UtcNow;

                var updated = await _printerRepository.UpdateAsync(printer);
                return ApiResponse<PrinterDetailViewModel?>.SuccessResult(MapToDetailViewModel(updated), "Printer updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PrinterDetailViewModel?>.ErrorResult($"Failed to update printer: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeletePrinterAsync(int id)
        {
            try
            {
                var printer = await _printerRepository.GetByIdAsync(id);
                if (printer == null)
                    return ApiResponse<bool>.ErrorResult("Printer not found");

                var result = await _printerRepository.DeleteAsync(id);
                return ApiResponse<bool>.SuccessResult(result, "Printer deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to delete printer: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ActivatePrinterAsync(int id, bool isActive)
        {
            try
            {
                var printer = await _printerRepository.GetByIdAsync(id);
                if (printer == null)
                    return ApiResponse<bool>.ErrorResult("Printer not found");

                printer.IsActive = isActive;
                printer.UpdatedAt = DateTime.UtcNow;
                await _printerRepository.UpdateAsync(printer);
                return ApiResponse<bool>.SuccessResult(true, $"Printer {(isActive ? "activated" : "deactivated")} successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to update printer status: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> IsNameUniqueAsync(string name, int? excludePrinterId = null)
        {
            try
            {
                var isUnique = await _printerRepository.IsNameUniqueAsync(name, excludePrinterId);
                return ApiResponse<bool>.SuccessResult(isUnique);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Failed to check printer name: {ex.Message}");
            }
        }

        private static PrinterListViewModel MapToListViewModel(Printer p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ConnectionInfo = p.ConnectionInfo,
            PrinterType = p.PrinterType,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt
        };

        private static PrinterDetailViewModel MapToDetailViewModel(Printer p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ConnectionInfo = p.ConnectionInfo,
            PrinterType = p.PrinterType,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        };
    }
}
