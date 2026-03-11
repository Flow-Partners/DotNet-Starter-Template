using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBridge.Models.Entities
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string? MedicalHistory { get; set; }

        public bool AllowPayLater { get; set; } = false;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CreditLimit { get; set; }

        [MaxLength(200)]
        public string? EmergencyContactName { get; set; }

        [MaxLength(50)]
        public string? EmergencyContactPhone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public virtual ICollection<PendingPayment> PendingPayments { get; set; } = new List<PendingPayment>();
    }
}
