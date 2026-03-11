namespace CareBridge.Models.DTOs.Dental
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DentistId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string ProcedureType { get; set; } = string.Empty;
        public int? ProcedureId { get; set; }
        public string Status { get; set; } = "Scheduled";
        public decimal EstimatedCost { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DentistName { get; set; } = string.Empty;
    }
}
