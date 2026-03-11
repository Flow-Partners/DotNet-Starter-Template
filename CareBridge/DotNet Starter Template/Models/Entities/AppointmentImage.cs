using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBridge.Models.Entities
{
    public class AppointmentImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public string ImageType { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; } = null!;
    }
}
