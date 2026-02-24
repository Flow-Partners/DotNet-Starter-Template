using Microsoft.EntityFrameworkCore;
using DotNet_Starter_Template.Data;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Repositories.Interfaces;

namespace DotNet_Starter_Template.Repositories.Implementations
{
    public class PrintingTemplateRepository : Repository<PrintingTemplate>, IPrintingTemplateRepository
    {
        public PrintingTemplateRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PrintingTemplate?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<IEnumerable<PrintingTemplate>> GetActiveTemplatesAsync()
        {
            return await _dbSet
                .Where(t => t.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<PrintingTemplate>> GetByTypeAsync(string templateType)
        {
            return await _dbSet
                .Where(t => t.TemplateType == templateType)
                .ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeTemplateId = null)
        {
            var query = _dbSet.Where(t => t.Name == name);
            if (excludeTemplateId.HasValue)
            {
                query = query.Where(t => t.Id != excludeTemplateId.Value);
            }
            return !await query.AnyAsync();
        }
    }
}
