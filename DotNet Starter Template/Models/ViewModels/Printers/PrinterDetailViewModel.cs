namespace DotNet_Starter_Template.Models.ViewModels.Printers
{
    public class PrinterDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ConnectionInfo { get; set; }
        public string PrinterType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
