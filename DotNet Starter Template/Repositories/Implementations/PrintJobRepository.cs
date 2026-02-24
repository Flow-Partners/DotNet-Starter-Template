using Microsoft.EntityFrameworkCore;
using DotNet_Starter_Template.Data;
using DotNet_Starter_Template.Models.Entities;
using DotNet_Starter_Template.Repositories.Interfaces;

namespace DotNet_Starter_Template.Repositories.Implementations
{
    public class PrintJobRepository : Repository<PrintJob>, IPrintJobRepository
    {
        public PrintJobRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PrintJob?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(pj => pj.Printer)
                .Include(pj => pj.PrintingTemplate)
                .Include(pj => pj.RequestedBy)
                .FirstOrDefaultAsync(pj => pj.Id == id);
        }

        public async Task<IEnumerable<PrintJob>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(pj => pj.Printer)
                .Include(pj => pj.PrintingTemplate)
                .Include(pj => pj.RequestedBy)
                .OrderByDescending(pj => pj.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PrintJob>> GetByPrinterIdAsync(int printerId)
        {
            return await _dbSet
                .Where(pj => pj.PrinterId == printerId)
                .Include(pj => pj.Printer)
                .Include(pj => pj.PrintingTemplate)
                .OrderByDescending(pj => pj.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PrintJob>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(pj => pj.Status == status)
                .Include(pj => pj.Printer)
                .Include(pj => pj.PrintingTemplate)
                .OrderByDescending(pj => pj.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PrintJob>> GetByRequestedByIdAsync(string userId)
        {
            return await _dbSet
                .Where(pj => pj.RequestedById == userId)
                .Include(pj => pj.Printer)
                .Include(pj => pj.PrintingTemplate)
                .OrderByDescending(pj => pj.CreatedAt)
                .ToListAsync();
        }
    }
}
