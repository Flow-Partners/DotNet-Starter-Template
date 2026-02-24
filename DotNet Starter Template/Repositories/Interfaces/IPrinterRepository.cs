using DotNet_Starter_Template.Models.Entities;

namespace DotNet_Starter_Template.Repositories.Interfaces
{
    public interface IPrinterRepository : IRepository<Printer>
    {
        Task<Printer?> GetByNameAsync(string name);
        Task<IEnumerable<Printer>> GetActivePrintersAsync();
        Task<bool> IsNameUniqueAsync(string name, int? excludePrinterId = null);
    }
}
