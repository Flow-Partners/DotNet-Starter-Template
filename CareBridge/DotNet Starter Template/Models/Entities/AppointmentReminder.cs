using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBridge.Models.Entities
{
    public class AppointmentReminder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        [MaxLength(20)]
        public string ReminderType { get; set; } = "Email";

        [Required]
        public DateTime ScheduledAt { get; set; }

        public DateTime? SentAt { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; } = null!;
    }
}
