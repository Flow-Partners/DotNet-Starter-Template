namespace CareBridge.Models.DTOs.Dental
{
    public class AppointmentImageDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string ImageType { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    public class CreateAppointmentImageDto
    {
        public int AppointmentId { get; set; }
        public string ImageType { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UpdateAppointmentImageDto
    {
        public string ImageType { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
