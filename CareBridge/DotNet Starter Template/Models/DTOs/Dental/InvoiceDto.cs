namespace CareBridge.Models.DTOs.Dental
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DentistId { get; set; }
        public string DentistName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? DueDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
