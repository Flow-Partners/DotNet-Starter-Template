using DotNet_Starter_Template.Models.Common;
using DotNet_Starter_Template.Models.DTOs.Printers;
using DotNet_Starter_Template.Models.ViewModels.Printers;

namespace DotNet_Starter_Template.Services.Interfaces
{
    public interface IPrinterService
    {
        Task<ApiResponse<PagedResult<PrinterListViewModel>>> GetAllPrintersAsync(PaginationRequest request);
        Task<ApiResponse<PrinterDetailViewModel?>> GetPrinterByIdAsync(int id);
        Task<ApiResponse<PrinterDetailViewModel>> CreatePrinterAsync(CreatePrinterDto createPrinterDto);
        Task<ApiResponse<PrinterDetailViewModel?>> UpdatePrinterAsync(int id, UpdatePrinterDto updatePrinterDto);
        Task<ApiResponse<bool>> DeletePrinterAsync(int id);
        Task<ApiResponse<bool>> ActivatePrinterAsync(int id, bool isActive);
        Task<ApiResponse<bool>> IsNameUniqueAsync(string name, int? excludePrinterId = null);
    }
}
