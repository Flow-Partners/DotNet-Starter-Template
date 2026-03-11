namespace CareBridge.Models.DTOs.Dental
{
    public class ProcedureDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal DefaultPrice { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
