using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBridge.Models.Entities
{
    /// <summary>
    /// Tracks pending (unpaid) balance for an invoice when patient pays partially.
    /// </summary>
    public class PendingPayment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Paid, Overdue, Cancelled

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PaidAt { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; } = null!;

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; } = null!;
    }
}
