namespace CareBridge.Models.DTOs.Dental
{
    public class CreateInvoiceDto
    {
        public int AppointmentId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Notes { get; set; }
        /// <summary>If true, creates a PendingPayment record for the full balance (e.g. pay later).</summary>
        public bool CreatePendingRecord { get; set; } = true;
    }
}
