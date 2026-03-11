using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBridge.Models.Entities
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DentistId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RemainingAmount { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime? DueDate { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; } = null!;

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; } = null!;

        [ForeignKey("DentistId")]
        public virtual Dentist Dentist { get; set; } = null!;

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<PendingPayment> PendingPayments { get; set; } = new List<PendingPayment>();
    }
}
