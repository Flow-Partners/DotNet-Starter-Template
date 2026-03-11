namespace CareBridge.Models.DTOs.Dental
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? MedicalHistory { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
