namespace CareBridge.Models.DTOs.Dental
{
    public class RecordPaymentDto
    {
        public decimal Amount { get; set; }
        public string Method { get; set; } = "Cash";
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
    }
}
