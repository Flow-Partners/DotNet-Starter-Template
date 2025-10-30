namespace DotNet_Starter_Template.Models.Configurations
{
    public class SerilogSettings
    {
        public string MinimumLevel { get; set; } = "Information";
        public List<SerilogSink> WriteTo { get; set; } = new List<SerilogSink>();
        public List<string> Enrich { get; set; } = new List<string>();
    }

    public class SerilogSink
    {
        public string Name { get; set; } = string.Empty;
        public Dictionary<string, object> Args { get; set; } = new Dictionary<string, object>();
    }
}

