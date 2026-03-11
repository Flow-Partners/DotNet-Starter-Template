using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareBridge.Data;
using CareBridge.Models.Entities;
using CareBridge.Models.DTOs.Dental;

namespace CareBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentImagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentImageDto>>> GetAppointmentImages()
        {
            var appointmentImages = await _context.AppointmentImages
                .Include(ai => ai.Appointment)
                .Select(ai => new AppointmentImageDto
                {
                    Id = ai.Id,
                    AppointmentId = ai.AppointmentId,
                    ImageType = ai.ImageType,
                    ImageUrl = ai.ImageUrl,
                    Description = ai.Description,
                    UploadedAt = ai.UploadedAt
                })
                .ToListAsync();

            return appointmentImages;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentImageDto>> GetAppointmentImage(int id)
        {
            var appointmentImage = await _context.AppointmentImages
                .Include(ai => ai.Appointment)
                .FirstOrDefaultAsync(ai => ai.Id == id);

            if (appointmentImage == null) return NotFound();

            var appointmentImageDto = new AppointmentImageDto
            {
                Id = appointmentImage.Id,
                AppointmentId = appointmentImage.AppointmentId,
                ImageType = appointmentImage.ImageType,
                ImageUrl = appointmentImage.ImageUrl,
                Description = appointmentImage.Description,
                UploadedAt = appointmentImage.UploadedAt
            };

            return appointmentImageDto;
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<IEnumerable<AppointmentImageDto>>> GetAppointmentImagesByAppointment(int appointmentId)
        {
            var appointmentImages = await _context.AppointmentImages
                .Where(ai => ai.AppointmentId == appointmentId)
                .Select(ai => new AppointmentImageDto
                {
                    Id = ai.Id,
                    AppointmentId = ai.AppointmentId,
                    ImageType = ai.ImageType,
                    ImageUrl = ai.ImageUrl,
                    Description = ai.Description,
                    UploadedAt = ai.UploadedAt
                })
                .ToListAsync();

            return appointmentImages;
        }

        [HttpGet("appointment/{appointmentId}/type/{imageType}")]
        public async Task<ActionResult<IEnumerable<AppointmentImageDto>>> GetAppointmentImagesByType(int appointmentId, string imageType)
        {
            var appointmentImages = await _context.AppointmentImages
                .Where(ai => ai.AppointmentId == appointmentId && ai.ImageType.ToLower() == imageType.ToLower())
                .Select(ai => new AppointmentImageDto
                {
                    Id = ai.Id,
                    AppointmentId = ai.AppointmentId,
                    ImageType = ai.ImageType,
                    ImageUrl = ai.ImageUrl,
                    Description = ai.Description,
                    UploadedAt = ai.UploadedAt
                })
                .ToListAsync();

            return appointmentImages;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointmentImage(int id, UpdateAppointmentImageDto updateAppointmentImageDto)
        {
            var appointmentImage = await _context.AppointmentImages.FindAsync(id);
            if (appointmentImage == null) return NotFound();

            appointmentImage.ImageType = updateAppointmentImageDto.ImageType;
            appointmentImage.ImageUrl = updateAppointmentImageDto.ImageUrl;
            appointmentImage.Description = updateAppointmentImageDto.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentImageExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentImageDto>> PostAppointmentImage(CreateAppointmentImageDto createAppointmentImageDto)
        {
            var appointmentImage = new AppointmentImage
            {
                AppointmentId = createAppointmentImageDto.AppointmentId,
                ImageType = createAppointmentImageDto.ImageType,
                ImageUrl = createAppointmentImageDto.ImageUrl,
                Description = createAppointmentImageDto.Description,
                UploadedAt = DateTime.UtcNow
            };

            _context.AppointmentImages.Add(appointmentImage);
            await _context.SaveChangesAsync();

            var appointmentImageDto = new AppointmentImageDto
            {
                Id = appointmentImage.Id,
                AppointmentId = appointmentImage.AppointmentId,
                ImageType = appointmentImage.ImageType,
                ImageUrl = appointmentImage.ImageUrl,
                Description = appointmentImage.Description,
                UploadedAt = appointmentImage.UploadedAt
            };

            return CreatedAtAction("GetAppointmentImage", new { id = appointmentImage.Id }, appointmentImageDto);
        }

        [HttpPost("upload-multiple")]
        public async Task<ActionResult<IEnumerable<AppointmentImageDto>>> PostMultipleAppointmentImages(List<CreateAppointmentImageDto> createAppointmentImageDtos)
        {
            if (createAppointmentImageDtos == null || createAppointmentImageDtos.Count == 0)
                return BadRequest("No images provided.");

            var appointmentImages = new List<AppointmentImage>();
            foreach (var createDto in createAppointmentImageDtos)
            {
                appointmentImages.Add(new AppointmentImage
                {
                    AppointmentId = createDto.AppointmentId,
                    ImageType = createDto.ImageType,
                    ImageUrl = createDto.ImageUrl,
                    Description = createDto.Description,
                    UploadedAt = DateTime.UtcNow
                });
            }

            _context.AppointmentImages.AddRange(appointmentImages);
            await _context.SaveChangesAsync();

            var resultDtos = appointmentImages.Select(ai => new AppointmentImageDto
            {
                Id = ai.Id,
                AppointmentId = ai.AppointmentId,
                ImageType = ai.ImageType,
                ImageUrl = ai.ImageUrl,
                Description = ai.Description,
                UploadedAt = ai.UploadedAt
            }).ToList();

            return CreatedAtAction("GetAppointmentImagesByAppointment", new { appointmentId = appointmentImages.First().AppointmentId }, resultDtos);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentImage(int id)
        {
            var appointmentImage = await _context.AppointmentImages.FindAsync(id);
            if (appointmentImage == null) return NotFound();

            _context.AppointmentImages.Remove(appointmentImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("appointment/{appointmentId}")]
        public async Task<IActionResult> DeleteAppointmentImagesByAppointment(int appointmentId)
        {
            var appointmentImages = await _context.AppointmentImages
                .Where(ai => ai.AppointmentId == appointmentId)
                .ToListAsync();

            if (appointmentImages == null || !appointmentImages.Any())
                return NotFound();

            _context.AppointmentImages.RemoveRange(appointmentImages);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentImageExists(int id) => _context.AppointmentImages.Any(e => e.Id == id);
    }
}
