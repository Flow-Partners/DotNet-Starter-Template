using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareBridge.Data;
using CareBridge.Models.Entities;
using CareBridge.Models.DTOs.Dental;

namespace CareBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices()
        {
            var list = await _context.Invoices
                .Include(i => i.Patient)
                .Include(i => i.Dentist)
                .Select(i => new InvoiceDto
                {
                    Id = i.Id,
                    AppointmentId = i.AppointmentId,
                    PatientId = i.PatientId,
                    PatientName = i.Patient.FirstName + " " + i.Patient.LastName,
                    DentistId = i.DentistId,
                    DentistName = i.Dentist.FirstName + " " + i.Dentist.LastName,
                    TotalAmount = i.TotalAmount,
                    PaidAmount = i.PaidAmount,
                    RemainingAmount = i.RemainingAmount,
                    Status = i.Status,
                    DueDate = i.DueDate,
                    Notes = i.Notes,
                    CreatedAt = i.CreatedAt
                })
                .ToListAsync();
            return list;
        }

        /// <summary>Create an invoice for an appointment. Optionally set total and due date.</summary>
        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> PostInvoice([FromBody] CreateInvoiceDto dto)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .FirstOrDefaultAsync(a => a.Id == dto.AppointmentId);
            if (appointment == null) return NotFound("Appointment not found.");
            var existing = await _context.Invoices.FirstOrDefaultAsync(i => i.AppointmentId == dto.AppointmentId);
            if (existing != null) return BadRequest("Invoice already exists for this appointment.");

            var total = dto.TotalAmount > 0 ? dto.TotalAmount : appointment.TotalAmount;
            if (total <= 0) total = appointment.EstimatedCost;

            var invoice = new Invoice
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                DentistId = appointment.DentistId,
                TotalAmount = total,
                PaidAmount = 0,
                RemainingAmount = total,
                Status = "Pending",
                DueDate = dto.DueDate,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            if (invoice.RemainingAmount > 0 && dto.CreatePendingRecord == true)
            {
                _context.PendingPayments.Add(new PendingPayment
                {
                    InvoiceId = invoice.Id,
                    PatientId = invoice.PatientId,
                    Amount = invoice.RemainingAmount,
                    DueDate = invoice.DueDate ?? DateTime.UtcNow.AddDays(30),
                    Status = "Pending",
                    Notes = "Initial balance",
                    CreatedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }

            await _context.Entry(invoice).Reference(i => i.Patient).LoadAsync();
            await _context.Entry(invoice).Reference(i => i.Dentist).LoadAsync();
            var result = new InvoiceDto
            {
                Id = invoice.Id,
                AppointmentId = invoice.AppointmentId,
                PatientId = invoice.PatientId,
                PatientName = invoice.Patient.FirstName + " " + invoice.Patient.LastName,
                DentistId = invoice.DentistId,
                DentistName = invoice.Dentist.FirstName + " " + invoice.Dentist.LastName,
                TotalAmount = invoice.TotalAmount,
                PaidAmount = invoice.PaidAmount,
                RemainingAmount = invoice.RemainingAmount,
                Status = invoice.Status,
                DueDate = invoice.DueDate,
                Notes = invoice.Notes,
                CreatedAt = invoice.CreatedAt
            };
            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
        {
            var i = await _context.Invoices
                .Include(inv => inv.Patient)
                .Include(inv => inv.Dentist)
                .FirstOrDefaultAsync(inv => inv.Id == id);
            if (i == null) return NotFound();
            return new InvoiceDto
            {
                Id = i.Id,
                AppointmentId = i.AppointmentId,
                PatientId = i.PatientId,
                PatientName = i.Patient.FirstName + " " + i.Patient.LastName,
                DentistId = i.DentistId,
                DentistName = i.Dentist.FirstName + " " + i.Dentist.LastName,
                TotalAmount = i.TotalAmount,
                PaidAmount = i.PaidAmount,
                RemainingAmount = i.RemainingAmount,
                Status = i.Status,
                DueDate = i.DueDate,
                Notes = i.Notes,
                CreatedAt = i.CreatedAt
            };
        }

        /// <summary>Record a payment against an invoice. If partial, creates/updates PendingPayment and keeps invoice status as Pending.</summary>
        [HttpPost("{id}/payments")]
        public async Task<ActionResult> RecordPayment(int id, RecordPaymentDto dto)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Patient)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null) return NotFound();
            if (dto.Amount <= 0) return BadRequest("Amount must be positive.");
            if (dto.Amount > invoice.RemainingAmount) return BadRequest("Amount exceeds remaining balance.");

            var payment = new Payment
            {
                InvoiceId = id,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                Method = dto.Method ?? "Cash",
                ReferenceNumber = dto.ReferenceNumber,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };
            _context.Payments.Add(payment);

            invoice.PaidAmount += dto.Amount;
            invoice.RemainingAmount = invoice.TotalAmount - invoice.PaidAmount;
            invoice.UpdatedAt = DateTime.UtcNow;

            if (invoice.RemainingAmount <= 0)
            {
                invoice.Status = "Paid";
                var pending = await _context.PendingPayments.Where(pp => pp.InvoiceId == id && pp.Status == "Pending").ToListAsync();
                foreach (var pp in pending)
                {
                    pp.Status = "Paid";
                    pp.PaidAt = DateTime.UtcNow;
                }
            }
            else
            {
                invoice.Status = "Pending";
                var dueDate = invoice.DueDate ?? DateTime.UtcNow.AddDays(30);
                var existing = await _context.PendingPayments.FirstOrDefaultAsync(pp => pp.InvoiceId == id && pp.Status == "Pending");
                if (existing != null)
                {
                    existing.Amount = invoice.RemainingAmount;
                    existing.DueDate = dueDate;
                }
                else
                {
                    _context.PendingPayments.Add(new PendingPayment
                    {
                        InvoiceId = id,
                        PatientId = invoice.PatientId,
                        Amount = invoice.RemainingAmount,
                        DueDate = dueDate,
                        Status = "Pending",
                        Notes = "Partial payment recorded.",
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
