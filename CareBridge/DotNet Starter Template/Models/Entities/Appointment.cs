using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBridge.Models.Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DentistId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string ProcedureType { get; set; } = string.Empty;

        public int? ProcedureId { get; set; }

        [Required]
        public string Status { get; set; } = "Scheduled";

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedCost { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; } = null!;

        [ForeignKey("DentistId")]
        public virtual Dentist Dentist { get; set; } = null!;

        [ForeignKey("ProcedureId")]
        public virtual Procedure? Procedure { get; set; }

        public virtual ICollection<AppointmentImage> AppointmentImages { get; set; } = new List<AppointmentImage>();
        public virtual Invoice? Invoice { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public virtual ICollection<AppointmentReminder> AppointmentReminders { get; set; } = new List<AppointmentReminder>();
    }
}
