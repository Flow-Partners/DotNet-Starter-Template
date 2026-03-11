using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareBridge.Data;
using CareBridge.Models.DTOs.Dental;

namespace CareBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PendingPaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PendingPaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Get all pending payments, or filter by patientId.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PendingPaymentDto>>> GetPendingPayments([FromQuery] int? patientId)
        {
            var query = _context.PendingPayments
                .Include(pp => pp.Patient)
                .Where(pp => pp.Status == "Pending" || pp.Status == "Overdue");

            if (patientId.HasValue)
                query = query.Where(pp => pp.PatientId == patientId.Value);

            var list = await query
                .Select(pp => new PendingPaymentDto
                {
                    Id = pp.Id,
                    InvoiceId = pp.InvoiceId,
                    PatientId = pp.PatientId,
                    PatientName = pp.Patient.FirstName + " " + pp.Patient.LastName,
                    Amount = pp.Amount,
                    DueDate = pp.DueDate,
                    Status = pp.Status,
                    Notes = pp.Notes,
                    CreatedAt = pp.CreatedAt,
                    PaidAt = pp.PaidAt
                })
                .ToListAsync();

            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PendingPaymentDto>> GetPendingPayment(int id)
        {
            var pp = await _context.PendingPayments
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pp == null) return NotFound();
            return new PendingPaymentDto
            {
                Id = pp.Id,
                InvoiceId = pp.InvoiceId,
                PatientId = pp.PatientId,
                PatientName = pp.Patient.FirstName + " " + pp.Patient.LastName,
                Amount = pp.Amount,
                DueDate = pp.DueDate,
                Status = pp.Status,
                Notes = pp.Notes,
                CreatedAt = pp.CreatedAt,
                PaidAt = pp.PaidAt
            };
        }
    }
}
