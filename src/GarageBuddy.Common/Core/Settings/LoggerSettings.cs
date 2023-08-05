namespace GarageBuddy.Common.Core.Settings
{
    using static GarageBuddy.Common.Constants.GlobalConstants;

    public class LoggerSettings
    {
        public string AppName { get; set; } = SystemName;

        public string ElasticSearchUrl { get; set; } = string.Empty;

        public bool WriteToFile { get; set; } = false;

        public bool StructuredConsoleLogging { get; set; } = false;

        public string MinimumLogLevel { get; set; } = "Information";
    }
}
