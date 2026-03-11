using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareBridge.Data;
using CareBridge.Models.Entities;
using CareBridge.Models.DTOs.Dental;

namespace CareBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
        {
            var patients = await _context.Patients
                .Select(p => new PatientDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    Phone = p.Phone,
                    DateOfBirth = p.DateOfBirth,
                    Address = p.Address,
                    MedicalHistory = p.MedicalHistory,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();
            return patients;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            var patientDto = new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                Phone = patient.Phone,
                DateOfBirth = patient.DateOfBirth,
                Address = patient.Address,
                MedicalHistory = patient.MedicalHistory,
                CreatedAt = patient.CreatedAt
            };
            return patientDto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, PatientDto patientDto)
        {
            if (id != patientDto.Id) return BadRequest();

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            patient.FirstName = patientDto.FirstName;
            patient.LastName = patientDto.LastName;
            patient.Email = patientDto.Email;
            patient.Phone = patientDto.Phone;
            patient.DateOfBirth = patientDto.DateOfBirth;
            patient.Address = patientDto.Address;
            patient.MedicalHistory = patientDto.MedicalHistory;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> PostPatient(PatientDto patientDto)
        {
            var patient = new Patient
            {
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                Email = patientDto.Email,
                Phone = patientDto.Phone,
                DateOfBirth = patientDto.DateOfBirth,
                Address = patientDto.Address,
                MedicalHistory = patientDto.MedicalHistory,
                CreatedAt = DateTime.UtcNow
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            patientDto.Id = patient.Id;
            patientDto.CreatedAt = patient.CreatedAt;
            return CreatedAtAction("GetPatient", new { id = patient.Id }, patientDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            var appointments = _context.Appointments.Where(a => a.PatientId == id);
            _context.Appointments.RemoveRange(appointments);

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PatientExists(int id) => _context.Patients.Any(e => e.Id == id);
    }
}
