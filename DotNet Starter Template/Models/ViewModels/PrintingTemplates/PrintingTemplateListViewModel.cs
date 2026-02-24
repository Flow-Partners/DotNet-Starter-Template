namespace DotNet_Starter_Template.Models.ViewModels.PrintingTemplates
{
    public class PrintingTemplateListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? TemplateType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
