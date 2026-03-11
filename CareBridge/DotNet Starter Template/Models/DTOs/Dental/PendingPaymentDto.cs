namespace CareBridge.Models.DTOs.Dental
{
    public class PendingPaymentDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
