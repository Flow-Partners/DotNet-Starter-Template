using DotNet_Starter_Template.Models.Entities;

namespace DotNet_Starter_Template.Repositories.Interfaces
{
    public interface IPrintJobRepository : IRepository<PrintJob>
    {
        Task<PrintJob?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<PrintJob>> GetAllWithDetailsAsync();
        Task<IEnumerable<PrintJob>> GetByPrinterIdAsync(int printerId);
        Task<IEnumerable<PrintJob>> GetByStatusAsync(string status);
        Task<IEnumerable<PrintJob>> GetByRequestedByIdAsync(string userId);
    }
}
