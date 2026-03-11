using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareBridge.Data;
using CareBridge.Models.DTOs.Dental;

namespace CareBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProceduresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProceduresController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Get all procedures, or only procedures available for a specific dentist when dentistId is provided.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcedureDto>>> GetProcedures([FromQuery] int? dentistId)
        {
            IQueryable<Models.Entities.Procedure> query = _context.Procedures.Where(p => p.IsActive);

            if (dentistId.HasValue)
            {
                var procedureIds = await _context.DentistProcedures
                    .Where(dp => dp.DentistId == dentistId.Value)
                    .Select(dp => dp.ProcedureId)
                    .ToListAsync();
                if (procedureIds.Count > 0)
                    query = query.Where(p => procedureIds.Contains(p.Id));
            }

            var list = await query
                .Select(p => new ProcedureDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    DefaultPrice = p.DefaultPrice,
                    Category = p.Category,
                    Description = p.Description,
                    IsActive = p.IsActive
                })
                .ToListAsync();

            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProcedureDto>> GetProcedure(int id)
        {
            var p = await _context.Procedures.FindAsync(id);
            if (p == null) return NotFound();
            return new ProcedureDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                DefaultPrice = p.DefaultPrice,
                Category = p.Category,
                Description = p.Description,
                IsActive = p.IsActive
            };
        }

        [HttpPost]
        public async Task<ActionResult<ProcedureDto>> PostProcedure(ProcedureDto dto)
        {
            var p = new Models.Entities.Procedure
            {
                Name = dto.Name,
                Code = dto.Code,
                DefaultPrice = dto.DefaultPrice,
                Category = dto.Category,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };
            _context.Procedures.Add(p);
            await _context.SaveChangesAsync();
            dto.Id = p.Id;
            return CreatedAtAction("GetProcedure", new { id = p.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcedure(int id, ProcedureDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var p = await _context.Procedures.FindAsync(id);
            if (p == null) return NotFound();
            p.Name = dto.Name;
            p.Code = dto.Code;
            p.DefaultPrice = dto.DefaultPrice;
            p.Category = dto.Category;
            p.Description = dto.Description;
            p.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedure(int id)
        {
            var p = await _context.Procedures.FindAsync(id);
            if (p == null) return NotFound();
            _context.Procedures.Remove(p);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
