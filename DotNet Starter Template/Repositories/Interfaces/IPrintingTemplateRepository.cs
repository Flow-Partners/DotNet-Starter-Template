using DotNet_Starter_Template.Models.Entities;

namespace DotNet_Starter_Template.Repositories.Interfaces
{
    public interface IPrintingTemplateRepository : IRepository<PrintingTemplate>
    {
        Task<PrintingTemplate?> GetByNameAsync(string name);
        Task<IEnumerable<PrintingTemplate>> GetActiveTemplatesAsync();
        Task<IEnumerable<PrintingTemplate>> GetByTypeAsync(string templateType);
        Task<bool> IsNameUniqueAsync(string name, int? excludeTemplateId = null);
    }
}
