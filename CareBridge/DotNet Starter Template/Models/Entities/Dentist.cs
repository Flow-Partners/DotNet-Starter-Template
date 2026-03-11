using System.ComponentModel.DataAnnotations;

namespace CareBridge.Models.Entities
{
    public class Dentist
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

        public string? Specialization { get; set; }

        public string? LicenseNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public virtual ICollection<DentistProcedure> DentistProcedures { get; set; } = new List<DentistProcedure>();
    }
}
