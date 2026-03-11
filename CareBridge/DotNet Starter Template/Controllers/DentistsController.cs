using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareBridge.Data;
using CareBridge.Models.Entities;
using CareBridge.Models.DTOs.Dental;

namespace CareBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DentistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DentistDto>>> GetDentists()
        {
            var dentists = await _context.Dentists
                .Select(d => new DentistDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Email = d.Email,
                    Phone = d.Phone,
                    Specialization = d.Specialization,
                    LicenseNumber = d.LicenseNumber,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();
            return dentists;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DentistDto>> GetDentist(int id)
        {
            var dentist = await _context.Dentists.FindAsync(id);
            if (dentist == null) return NotFound();

            var dentistDto = new DentistDto
            {
                Id = dentist.Id,
                FirstName = dentist.FirstName,
                LastName = dentist.LastName,
                Email = dentist.Email,
                Phone = dentist.Phone,
                Specialization = dentist.Specialization,
                LicenseNumber = dentist.LicenseNumber,
                CreatedAt = dentist.CreatedAt
            };
            return dentistDto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDentist(int id, DentistDto dentistDto)
        {
            if (id != dentistDto.Id) return BadRequest();

            var dentist = await _context.Dentists.FindAsync(id);
            if (dentist == null) return NotFound();

            dentist.FirstName = dentistDto.FirstName;
            dentist.LastName = dentistDto.LastName;
            dentist.Email = dentistDto.Email;
            dentist.Phone = dentistDto.Phone;
            dentist.Specialization = dentistDto.Specialization;
            dentist.LicenseNumber = dentistDto.LicenseNumber;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DentistExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<DentistDto>> PostDentist(DentistDto dentistDto)
        {
            var dentist = new Dentist
            {
                FirstName = dentistDto.FirstName,
                LastName = dentistDto.LastName,
                Email = dentistDto.Email,
                Phone = dentistDto.Phone,
                Specialization = dentistDto.Specialization,
                LicenseNumber = dentistDto.LicenseNumber,
                CreatedAt = DateTime.UtcNow
            };

            _context.Dentists.Add(dentist);
            await _context.SaveChangesAsync();

            dentistDto.Id = dentist.Id;
            dentistDto.CreatedAt = dentist.CreatedAt;
            return CreatedAtAction("GetDentist", new { id = dentist.Id }, dentistDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDentist(int id)
        {
            var dentist = await _context.Dentists.FindAsync(id);
            if (dentist == null) return NotFound();

            _context.Dentists.Remove(dentist);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>Set which procedures this dentist can perform. Replaces existing assignment.</summary>
        [HttpPut("{id}/procedures")]
        public async Task<IActionResult> SetDentistProcedures(int id, [FromBody] int[] procedureIds)
        {
            var dentist = await _context.Dentists.FindAsync(id);
            if (dentist == null) return NotFound();

            var existing = await _context.DentistProcedures.Where(dp => dp.DentistId == id).ToListAsync();
            _context.DentistProcedures.RemoveRange(existing);

            var validIds = await _context.Procedures.Where(p => procedureIds.Contains(p.Id)).Select(p => p.Id).ToListAsync();
            foreach (var pid in validIds)
                _context.DentistProcedures.Add(new DentistProcedure { DentistId = id, ProcedureId = pid });

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool DentistExists(int id) => _context.Dentists.Any(e => e.Id == id);
    }
}
