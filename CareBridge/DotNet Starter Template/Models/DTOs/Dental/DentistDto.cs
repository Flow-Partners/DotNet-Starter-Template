namespace CareBridge.Models.DTOs.Dental
{
    public class DentistDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Specialization { get; set; }
        public string? LicenseNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
