using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareBridge.Data;
using CareBridge.Models.Entities;
using CareBridge.Models.DTOs.Dental;

namespace CareBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DentistId = a.DentistId,
                    AppointmentDate = a.AppointmentDate,
                    ProcedureType = a.ProcedureType,
                    ProcedureId = a.ProcedureId,
                    Status = a.Status,
                    EstimatedCost = a.EstimatedCost,
                    TotalAmount = a.TotalAmount,
                    Notes = a.Notes,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    PatientName = $"{a.Patient.FirstName} {a.Patient.LastName}",
                    DentistName = $"{a.Dentist.FirstName} {a.Dentist.LastName}"
                })
                .ToListAsync();

            return appointments;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) return NotFound();

            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DentistId = appointment.DentistId,
                AppointmentDate = appointment.AppointmentDate,
                ProcedureType = appointment.ProcedureType,
                ProcedureId = appointment.ProcedureId,
                Status = appointment.Status,
                EstimatedCost = appointment.EstimatedCost,
                TotalAmount = appointment.TotalAmount,
                Notes = appointment.Notes,
                CreatedAt = appointment.CreatedAt,
                UpdatedAt = appointment.UpdatedAt,
                PatientName = $"{appointment.Patient.FirstName} {appointment.Patient.LastName}",
                DentistName = $"{appointment.Dentist.FirstName} {appointment.Dentist.LastName}"
            };

            return appointmentDto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, AppointmentDto appointmentDto)
        {
            if (id != appointmentDto.Id) return BadRequest();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.PatientId = appointmentDto.PatientId;
            appointment.DentistId = appointmentDto.DentistId;
            appointment.AppointmentDate = appointmentDto.AppointmentDate;
            appointment.ProcedureType = appointmentDto.ProcedureType;
            appointment.ProcedureId = appointmentDto.ProcedureId;
            appointment.Status = appointmentDto.Status;
            appointment.EstimatedCost = appointmentDto.EstimatedCost;
            appointment.TotalAmount = appointmentDto.TotalAmount;
            appointment.Notes = appointmentDto.Notes;
            appointment.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> PostAppointment(AppointmentDto appointmentDto)
        {
            var totalAmount = appointmentDto.TotalAmount > 0 ? appointmentDto.TotalAmount : appointmentDto.EstimatedCost;
            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DentistId = appointmentDto.DentistId,
                AppointmentDate = appointmentDto.AppointmentDate,
                ProcedureType = appointmentDto.ProcedureType,
                ProcedureId = appointmentDto.ProcedureId,
                Status = appointmentDto.Status,
                TotalAmount = totalAmount,
                EstimatedCost = appointmentDto.EstimatedCost,
                Notes = appointmentDto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            await _context.Entry(appointment).Reference(a => a.Patient).LoadAsync();
            await _context.Entry(appointment).Reference(a => a.Dentist).LoadAsync();

            appointmentDto.Id = appointment.Id;
            appointmentDto.CreatedAt = appointment.CreatedAt;
            appointmentDto.PatientName = $"{appointment.Patient.FirstName} {appointment.Patient.LastName}";
            appointmentDto.DentistName = $"{appointment.Dentist.FirstName} {appointment.Dentist.LastName}";

            return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointmentDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(int id) => _context.Appointments.Any(e => e.Id == id);
    }
}
