using Microsoft.EntityFrameworkCore;
using DotNet_Starter_Template.Data;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Repositories.Interfaces;

namespace DotNet_Starter_Template.Repositories.Implementations
{
    public class PrinterRepository : Repository<Printer>, IPrinterRepository
    {
        public PrinterRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Printer?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Printer>> GetActivePrintersAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludePrinterId = null)
        {
            var query = _dbSet.Where(p => p.Name == name);
            if (excludePrinterId.HasValue)
            {
                query = query.Where(p => p.Id != excludePrinterId.Value);
            }
            return !await query.AnyAsync();
        }
    }
}
