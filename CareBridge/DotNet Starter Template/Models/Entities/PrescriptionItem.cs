using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBridge.Models.Entities
{
    public class PrescriptionItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PrescriptionId { get; set; }

        [Required]
        [MaxLength(200)]
        public string MedicationName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Dosage { get; set; }

        [MaxLength(100)]
        public string? Frequency { get; set; }

        [MaxLength(100)]
        public string? Duration { get; set; }

        public string? Instructions { get; set; }

        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; } = null!;
    }
}
